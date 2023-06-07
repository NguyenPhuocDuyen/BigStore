namespace BigStore.BusinessObject.OtherModels
{
    public class Summernote
    {

        public Summernote(string IdEditor, bool LoadLibrary = true)
        {
            this.IdEditor = IdEditor;
            this.LoadLibrary = LoadLibrary;
        }

        public string IdEditor { get; set; }
        public bool LoadLibrary { get; set; }
        public int Height { get; set; } = 120;
        public string Toolbar { get; set; } = @"
            [
                ['style', ['style']],
                ['font', ['bold', 'underline', 'clear']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['table', ['table']],
                ['insert', ['link', 'picture', 'video']],
                ['height',['height']],
                ['view', ['fullscreen', 'codeview', 'help']]
            ]";
    }
}
