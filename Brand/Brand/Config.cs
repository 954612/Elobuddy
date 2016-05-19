using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass
namespace BrandBrand
{
    // I can't really help you with my layout of a good config class
    // since everyone does it the way they like it most, go checkout my
    // config classes I make on my GitHub if you wanna take over the
    // complex way that I use
    public static class Config
    {
        private const string MenuName = "BrandBrand";

        public static Menu Menu { get; set; }
        public static Menu ConfigMenu { get; set; }
        public static Menu ComboMenu { get; set; }
        public static Menu HarassMenu { get; set; }
        public static Menu LaneClearMenu { get; set; }
        public static Menu KsMenu { get; set; }
        public static Menu DrawMenu { get; set; }

        static Config()
        {
            // Initialize the menu
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Welcome to BrandBrand!");

            ConfigMenu = Menu.AddSubMenu("Config");


            ComboMenu = Menu.AddSubMenu("Combo");
            ComboMenu.Add("comboQ", new CheckBox("Use Q"));
            ComboMenu.Add("comboW", new CheckBox("Use W"));
            ComboMenu.Add("comboE", new CheckBox("Use E"));
            ComboMenu.Add("comboR", new CheckBox("Use R"));
            ComboMenu.Add("minEnemiesR", new Slider("Minimum enemies to use R", 1, 1, 6));
            
            HarassMenu = Menu.AddSubMenu("Harass");
            HarassMenu.AddGroupLabel("Spells");
            HarassMenu.Add("harassQ", new CheckBox("Use Q"));
            HarassMenu.Add("harassQmana", new Slider("Mininum mana % to use Q", 15, 0, 100));
            HarassMenu.AddSeparator();
            HarassMenu.Add("harassW", new CheckBox("Use W"));
            HarassMenu.Add("harassWmana", new Slider("Mininum mana % to use W", 15, 0, 100));
            HarassMenu.AddSeparator();
            HarassMenu.Add("harassE", new CheckBox("Use E"));
            HarassMenu.Add("spreadE", new CheckBox("Spread E on blazed minions"));
            HarassMenu.Add("harassEmana", new Slider("Mininum mana % to use E", 15, 0, 100));

            LaneClearMenu = Menu.AddSubMenu("Lane Clear");
            LaneClearMenu.AddGroupLabel("Spells");
            LaneClearMenu.Add("laneClearW", new CheckBox("Use Q"));
            LaneClearMenu.Add("minMinionsW", new Slider("Minimum minions to use Q", 2, 1, 6));
            LaneClearMenu.Add("minManaW", new Slider("Mininum mana % to use Q", 25, 0, 100));
            LaneClearMenu.AddSeparator();
            LaneClearMenu.Add("laneClearW", new CheckBox("Use W"));
            LaneClearMenu.Add("minMinionsW", new Slider("Minimum minions to use W", 2, 1, 6));
            LaneClearMenu.Add("minManaW", new Slider("Mininum mana % to use W", 25, 0, 100));
            LaneClearMenu.AddSeparator();
            LaneClearMenu.Add("laneClearE", new CheckBox("Use E"));
            LaneClearMenu.Add("minMinionsE", new Slider("Minimum minions to use E", 2, 1, 6));
            LaneClearMenu.Add("minManaE", new Slider("Mininum mana % to use E", 25, 0, 100));

            KsMenu = Menu.AddSubMenu("Killsteal");
            KsMenu.AddGroupLabel("Killsteal");
            KsMenu.Add("ksQ", new CheckBox("Use Q"));
            KsMenu.Add("ksW", new CheckBox("Use W"));
            KsMenu.Add("ksE", new CheckBox("Use E"));
            KsMenu.Add("ksR", new CheckBox("Use R"));
            KsMenu.Add("ksIgnite", new CheckBox("Use Ignite"));

            DrawMenu = Menu.AddSubMenu("Drawings");
            DrawMenu.Add("enbDraw", new CheckBox("Enable Drawings?"));
            DrawMenu.Add("drawQ", new CheckBox("Draw Q"));
            DrawMenu.Add("drawW", new CheckBox("Draw W"));
            DrawMenu.Add("drawE", new CheckBox("Draw E"));
            DrawMenu.Add("drawR", new CheckBox("Draw R"));
        }

        

        public static void Initialize()
        {
        }

    }
}
