using BigStore.BusinessObject;

namespace BigStore.Utility
{
    public static class SelectItem
    {
        public static void CreateSelectItems(List<Category> source, List<Category> des, int level)
        {
            string prefix = String.Concat(Enumerable.Repeat("—", level));
            foreach (var category in source)
            {
                des.Add(new Category
                {
                    Id = category.Id,
                    Title = prefix + " " + category.Title
                });
                if (category.CategoryChildren?.Count > 0)
                {
                    CreateSelectItems(category.CategoryChildren.ToList(), des, level + 1);
                }
            }
        }
    }
}
