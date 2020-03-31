namespace Percolate.Attributes
{
    /*
     * This enum represents a three state boolean value.
     * We cannot use boolean because attributes cannot have nullable properties, and so we can't know 
     * if a developer actually assigned false to a setting on the attribute or if they just left it blank.
     * But the validator needs to know so it can properly determine against which rule to validate the query parameters.
     */
    public enum PercolateAttributeSetting
    {
        Unset,
        Enabled,
        Disabled
    }
}
