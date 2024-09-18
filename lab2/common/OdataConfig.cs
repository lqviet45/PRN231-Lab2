using lab2.models;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace lab2.common;

public static class OdataConfig
{
    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<Book>("Books");
        builder.EntitySet<Press>("Presses");
        return builder.GetEdmModel();
    }
}