namespace Percolate
{
    /*
     * This enum represents a "three state boolean".
     * We cannot use a nullable boolean because attributes cannot have nullable properties
     * Thus, we don't know if a developer actually assigned false to a setting on the attribute,
     * or if they just didn't assign it (because false is the default for boolean).
     * The validator needs to know this so it can properly determine against which rule to validate the relevant query parameters.
     */
    public enum PercolateAttributeSetting
    {
        Unset,
        Enabled,
        Disabled
    }
}
