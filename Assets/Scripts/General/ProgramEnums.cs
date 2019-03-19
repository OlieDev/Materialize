namespace General
{
    public static class ProgramEnums
    {
        public enum FileFormat
        {
            Png,
            Jpg,
            Tga,
            Exr,
            Bmp,
            Invalid
        }

        public enum MapType
        {
            None,
            Any,
            Height,
            Diffuse,
            DiffuseOriginal,
            AnyDiffuse,
            Metallic,
            Smoothness,
            Normal,
            Ao,
            Property,
            MaskMap
        }

        public enum ScreenMode
        {
            FullScreen,
            Windowed
        }

        public enum GraphicsQuality
        {
            High,
            Medium,
            Low
        }
    }
}