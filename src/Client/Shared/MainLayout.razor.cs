using MudBlazor;

namespace Squads.Client.Shared
{
    public partial class MainLayout
    {
        bool _drawerOpen = true;
        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }
        // #f4f4f4 soft grey
        // #040606 soft black
        // #7c7d7d Gray web
        // #404242 Onyx

        private bool _isDarkMode;
        private static MudBlazor.Utilities.MudColor Onyx = new("#404242"); 
        private static MudBlazor.Utilities.MudColor Softgrey = new("#F4F4F4");
        private static MudBlazor.Utilities.MudColor GrayWeb = new("#7c7d7d");
        MudTheme _theme = new MudTheme()
        {
            Palette = new Palette()
            {
                Primary = Onyx,
                PrimaryContrastText = Softgrey,
                Secondary = Softgrey,
                SecondaryContrastText = Onyx,
                Tertiary = GrayWeb,
                AppbarBackground = Onyx,
                AppbarText = Softgrey,
                DrawerBackground = Softgrey,
                DrawerText = Onyx
            },
            /*            
             *  PaletteDark = new Palette()
             *  { Primary = Colors.Blue.Lighten1 },
             */
            Typography = new Typography()
            {
                Default = new Default()
                {
                   //TODO:
                    FontFamily = new[] { "Gotham-Medium", "Gotham-Light", "Helvetica","Arial" }
                }
            }
        };
    }
}
