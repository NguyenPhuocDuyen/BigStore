using BigStore.BusinessObject;

namespace BigStore.Utility
{
    public static class SelectItem
    {
        public static List<Category> CreateSelectItemsHasNoParent(List<Category> source, int level = 0)
        {
            source.Insert(0, new Category()
            {
                Id = "-1",
                Title = "Không có danh mục cha"
            });

            var items = new List<Category>();
            CreateSelectItems(source, items, level);
            return items;
        }

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
