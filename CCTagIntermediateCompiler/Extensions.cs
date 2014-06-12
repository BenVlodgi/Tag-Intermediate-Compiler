namespace CCTagIntermediateCompiler
{
    using System.Linq;
    using VMFParser;

    static class Extensions
    {
        /// <summary>Gets a unique entity identifier.</summary>
        /// This is a terrible implementaion, we are Parsing those ids so many times if we dont find the right id right away.
        /// Should create something like this in the VMFParser
        public static int GetUniqueEntityID(this VMF vmf)
        {
            int id = 0;
            var last = vmf.Body.LastOrDefault(entry => entry.Name == "entity") as VBlock;
            if (last == null)
                id = 100; //for fun
            else
            {
                var idProperty = last.Body.Where(property => property.GetType() == typeof(VProperty) && property.Name == "id").FirstOrDefault() as VProperty;
                if (int.TryParse(idProperty.Value, out id))
                {
                    //make sure this is not already used
                    string idCheck = id.ToString();
                    while (vmf.Body.Where(entry =>
                        entry.GetType() == typeof(VBlock) &&
                        entry.Name == "entity" &&
                        ((VBlock)entry).Body.Where(property =>
                            property.Name == "id" &&
                            property.GetType() == typeof(VProperty) &&
                            ((VProperty)property).Value == idCheck)
                            .Count() > 0)
                        .Count() > 0)
                        id = (id * 10) + 1;
                }
                else
                    id = 100;
            }

            return id;
        }
    }
}