# Percolate ☕

Percolate is a lightweight library for paging, filtering, and sorting your API responses.

## Table of Contents

- [Installation](#Installation)
- [Configuration](#Configuration)
- [API Usage](#API-Usage)
- [Client Usage](#Client-Usage)
  - [Paging](#Paging)
  - [Sorting](#Sorting)
  - [Filtering](#Filtering)

## Installation

_There's nothing here._

## Configuration

### Model Configuration

For Percolate to understand how to process your responses, it must know about the types of objects you'll be returning and any rules you'd like to apply to them.

You can do this by creating a class that implements the `PercolateModel` class. Let's say you want Percolate to be able to process responses of the type `Person`.

Here is the `Person` class:

```C#
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}
```

And here is the model class:

```C#
public class MyPercolateModel : PercolateModel
{
    public override void Configure(PercolateModelBuilder modelBuilder)
    {
        modelBuilder.Type<Person>();
    }
}
```

That's all you have to do to enable Percolate for any given type. The type will inherit any global rules and configuration you've set.

You can also override any configuration using a fluent style API. Say you have a class that you want to limit the page size for because it's really big:

```C#
modelBuilder.Type<MyReallyBigClass>()
    .HasMaxPageSize(10);
```

Or say you have a type that you don't want to allow certain operations on:

```C#
modelBuilder.Type<MyNonSortableNonFilterableClass>()
    .CanSort(false)
    .CanFilter(false);
```

You can also configure Percolate to disallow sorting and filtering on certain properties of your type:

```C#
modelBuilder.Type<Person>()
    .Property(p => p.Name)
    .CanSort(false);
```

The above code will prohibit Percolate from sorting based on the `Name` property.

### Service Registration

Once you have added the Percolate NuGet package and configured your model, you can use Percolate in your API by modifying the `ConfigureServices` method in your `Startup` class.

```C#
using Percolate;

public class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddPercolate<MyPercolateModel>();
    }
}
```

You can also configure any global options with a lambda:

```C#
services.AddPercolate<MyPercolateModel>(options => {
    options.IsPercolateEnabledGlobally = false;
    options.DoExceptionsFailSilently = true;
    options.DoResponsesIncludeMetadata = true;
    options.IsPagingEnabled = true;
    options.IsSortingEnabled = true;
    options.IsFilteringEnable = true;
    options.DefaultPageSize = 100;
    options.MaximumPageSize = 1000;
});
```

These global options can be overriden by both the configuration in your `MyPercolateModel` class and by the `EnablePercolate` and `DisablePercolate` attributes.

## API Usage

Percolate provides two ways to control the processing of actions: the `IsPercolateEnabledGlobally` global configuration options, and the `EnablePercolate` and `DisablePercolate` attributes.

If `IsPercolateEnabledGlobally` is set to true, Percolate will attempt to process every action in every controller, unless the action or controller are decorated with the `DisablePercolate` attribute. This is useful if you have only a few actions and you know all of them will successfully be processed by Percolate.

The more granular approach is to use the `EnablePercolate` attribute on actions and controllers. If you want an action to be processed by Percolate, all you need to do is add the attribute:

```C#
public class PersonController : ControllerBase
{
    [EnablePercolate]
    public IActionResult Get()
    {
        var people = MyGetPeopleMethod();

        return Ok(people);
    }
}
```

You can also enable Percolate for every action in a controller:

```C#
[EnablePercolate]
public class PersonController : ControllerBase
{
    public IActionResult Get()
    {
        var people = MyGetPeopleMethod();

        return Ok(people);
    }
}
```

This will cause Percolate to intercept the `OkObjectResult` and attempt to process it. Percolate is only able to process responses that meet the following criteria:

- The response is an `OkObjectResult`
- The value of the response implements `IEnumerable`

If those two criteria are met, Percolate will attempt parse the relevant query parameters from the request, validate them, and apply any changes to the results. If `DoExceptionsFailSilently` is set to true in the global options, and an exception does occur (for example, because of a malformed query parameter), Percolate will stop processing and return the original result.

You can configure Percolate on the attribute level as well:

```C#
[EnablePercolate(DefaultPageSize = 25, SortSetting = PercolateAttributeSetting.Disabled)]
```

This will configure the actions to have a default page size of 25 (in the case that no page size is provided by a query parameter) and will disallow sorting.

Percolate prioritizes the "lowest" level of configuration provided:

- If configuration is provided on the attribute, it overrides any higher level configuration (model definition and global options).
- If configuration is provided in the model definition, it overrides any higher level configuration (global options).
- The global options are the topmost level of configuration available; everything will inherit from them when no lower level configuration is provided.

## Client Usage

Percolate allows client applications to call your API with certain query parameters that will manipulate the results, without forcing you to implement the logic to handle those query parameters on every action.

Currently, Percolate supports paging, sorting, and filtering results.

### Paging

Paging allows you control the size of your response. If you have thousands of records, you don't necessarily want to serialize and return all of them.

Percolate can parse and apply the `page` and `pageSize` query parameters to make this easy.

Paging is implemented with the following rules and effects:

- `page` and `pageSize` must both be non-negative integers and they cannot exceed the maximum value of `Int32`.
- If the requested page is empty, Percolate will return an empty collection.
- Percolate will use a default `page` parameter of `1` if `page` is not present and a whatever the configured `DefaultPageSize` is if `pageSize` is not present.
- Paging is applied _after_ filtering and sorting.

### Sorting

Sorting allows you to control the order of the response.

Percolate parses the `sort` query parameter to achieve this.

Sorting has the following rules and effects:

- The `sort` query parameters is a comma deliniated list of the properties upon which you'd like to sort: `sort=name,-age`.
- If you want to sort by a property ascending, just put the property name. If you'd like to sort descending, put a `-` character in front of the property name.
- Sorts are applied in the order they are listed in the query parameter. If you pass `age,-age`, the `-age` will override the `age` sort.
- Sorting is applied _after_ filtering but _before_ paging.

### Filtering

Filtering allows you filter down your result set.

Percolate parses the `filter` query parameter to achieve this.

The `filter` query parameter follows this format:

```
filter=<property1> <and/or> <property2> <operator> <value1> or <value2>,<nextNode>,...
```

The nodes are comma separated. Within each node, you can have a filter operator check on any number of properties and values. The properties can be chained with logical `AND` or `OR` operators to allow complex logic. Nodes are always understood as being joined by a logical `AND`.

The available filter operators are:
| Operator | Name | Meaning |
| ---------|------|-------- |
| `eq` | Equal | Evaluates if the values of any of the provided properties are exactly equal to any of the provided values. If the property is a string, this is a case-sensitive comparison.
| `ne` | Not Equal | Evaluates if the values of any of the provided properties are not equal to any of the provided values. If the property is a string, this is a case-sensitive comparison.
| `gt` | Greater than | Evaluates if the values of any of the provided properties are greater than any of the provided values.
| `ge` | Greater than or equal to | Evaluates if the values of any of the provided properties are greater than or equal to any of the provided values.
| `lt` | Less than | Evaluates if the values of any of the provided properties are less than any of the provided values.
| `le` | Less than or equal to | Evaluates if the values of any of the provided properties are less than or equal to any of the provided values.

Percolate translates the query parameter into a LINQ expression. We'll use our `People` class to illustrate some examples of how it would understand certain parameters:

`filter=name eq Jane`:

```C#
people.Where(p => p.name == "Jane");
```

`filter=name eq Jane,age gt 30`:

```C#
people.Where(p => p.Name == "Jane" && p.age > 30);
```

`filter=name or title ne Baron`:

```C#
people.Where(p => p.name != "Tyler" || p.title != Baron);
```

`filter=name and title eq King or Queen`

```C#
people.Where(p => (p.name == "King" || p.name == "Queen") && (p.title == "King" || p.title == "Queen"));
```

You can also pass in strings by using single quotes (you must escape commas, single quotes if they are a part of your string. For best results, percent encode any characters you're concerned about): `filter=comment eq 'Hello\, World!'`
