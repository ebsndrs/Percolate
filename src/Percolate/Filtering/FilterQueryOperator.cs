namespace Percolate.Filtering
{
    public enum FilterQueryNodeOperator
    {
        None,
        IsEqual,
        CaseInsensitiveIsEqual,
        IsNotEqual,
        CaseInsensitiveIsNotEqual,
        IsGreaterThan,
        IsGreaterThanOrEqual,
        IsLessThan,
        IsLessThanOrEqual,
        DoesContain,
        CaseInsensitiveDoesContain,
        DoesNotContain,
        CaseInsensitiveDoesNotContain,
        DoesStartWith,
        CaseInsensitiveDoesStartWith,
        DoesNotStartWith,
        CaseInsensitiveDoesNotStartWith,
        DoesEndWith,
        CaseInsensitiveDoesEndWith,
        DoesNotEndWith,
        CaseInsensitiveDoesNotEndWith
    }
}
