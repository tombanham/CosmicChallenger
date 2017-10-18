namespace Assets.scripts.Services.LayoutGeneration
{
    public interface IAsteroidLayout
    {
        AsteroidLayoutName AsteroidLayoutName { get; set; }
        void ApplyLayout();
    }
}