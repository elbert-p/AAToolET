using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AATool.Data.Objectives.Complex;
using AATool.UI.Screens;
using Microsoft.Xna.Framework;

namespace AATool.UI.Controls
{
    class UIPointsBar : UIPanel
    {
        //biomes
        private const string AT = "minecraft:adventure/adventuring_time"; //adv id
        //crit
        private const string BJH = "minecraft:bamboo_jungle_hills";
        private const string GTTH = "minecraft:giant_tree_taiga_hills";
        private const string BP = "minecraft:badlands_plateau";
        private const string SnowyBeach = "minecraft:snowy_beach";
        private const string SnowyTaigaH = "minecraft:snowy_taiga_hills";
        private const string SnowyTundra = "minecraft:snowy_tundra";
        private const string SnowyMountains = "minecraft:snowy_mountains";

        // public override string AdvancementId = "minecraft:adventure/adventuring_time";
        // public override string Criterion => "Biome";

        //breeding
        // <criterion id="minecraft:mooshroom"/>

        private const string Breed = "minecraft:husbandry/bred_all_animals";
        private const string Mooshroom = "minecraft:mooshroom";
        private const string Ocelot = "minecraft:ocelot";
        private const string Wolf = "minecraft:wolf";
        private const string Fox = "minecraft:fox";
        private const string Panda = "minecraft:panda";

        //kills
        private const string Kills = "minecraft:adventure/kill_all_mobs";
        private const string CaveSpider = "minecraft:cave_spider";
        private const string Stray = "minecraft:stray";
        private const string Zoglin = "minecraft:zoglin";
        private const string Wither = "minecraft:wither"; //WitherRose = "minecraft:wither_rose";

        //advancements 
        private const string ZombieDoctor = "minecraft:story/cure_zombie_villager";
        private const string TwoBirds = "minecraft:adventure/two_birds_one_arrow";
        private const string VVF = "minecraft:adventure/very_very_frightening";
        private const string Arbalistic = "minecraft:adventure/arbalistic";
        private const string HDWGH = "minecraft:nether/all_effects";

        private const string UseLodestone = "minecraft:nether/use_lodestone";

        //cats        
        private const string Cats = "minecraft:husbandry/complete_catalogue";
        private const string BlackCat = "textures/entity/cat/all_black";

        //item counts
        private const string Shell = "minecraft:nautilus_shell";
        private const string Skull = "minecraft:wither_skeleton_skull";
        private const string Debris = "minecraft:ancient_debris";
        private const string Gold = "minecraft:gold_block";


        private const string Rocket = "minecraft:firework_rocket";
        private const string Poison = "minecraft:poisonous_potato";
        private const string Tnt = "minecraft:tnt";
        private const string Obsidian = "minecraft:obsidian";
        private const string Shell = "minecraft:nautilus_shell";
        private const string Skull = "minecraft:wither_skull";
        private const string Debris = "minecraft:ancient_debris";
        private const string Beehive = "minecraft:bee_nest";

        public readonly Dictionary<(string adv, string crit), double> PointCriteria = 
        new Dictionary<(string adv, string crit), double>
        {
            { (AT, BJH), 1 },
            { (AT, GTTH), 1 },
            { (AT, BP), 1 },
            { (AT, SnowyBeach), 1 },
            { (AT, SnowyTaigaH), 1 },
            { (AT, SnowyTundra), 1 },
            { (AT, SnowyMountains), 1 },
            { (Breed, Mooshroom), 3 },
            { (Breed, Ocelot), 1 },
            { (Breed, Wolf), 1 },
            { (Breed, Fox), 1 },
            { (Breed, Panda), 1 },
            { (Kills, CaveSpider), 1 },
            { (Kills, Stray), 1 },
            { (Cats, BlackCat), 0.5 },
            
        };

        //points panel
        private UITextBlock estimatedTime;
        private UITextBlock pointsLabel;
        private UIProgressBar pointsBar;


        public UIPointsBar()
        {
            this.BuildFromTemplate();
        }

        public override void InitializeThis(UIScreen screen)
        {
            base.InitializeThis(screen);

            //points panel
            //this.pointsPanel = this.First("points_panel");
            this.estimatedTime = this.First<UITextBlock>("estimated_time");
            this.estimatedTime.SetFont("minecraft", 24);
            this.pointsLabel = this.First<UITextBlock>("bar_label");
            this.pointsLabel.SetFont("minecraft", 24);
            this.pointsBar = this.First<UIProgressBar>();
            this.pointsBar.SetMin(0);
            this.pointsBar.SetMax(1);

        }

        public override void ResizeRecursive(Rectangle rectangle)
        {
            //this.progressBar.SkipToValue(Tracker.Category.GetCompletionRatio());
            base.ResizeRecursive(rectangle);
        }

        protected override void UpdateThis(Time time)
        {
            base.UpdateThis(time);
            if (Tracker.Invalidated)
                this.Refresh();
        }

        private void Refresh()
        {
            //this.First<UITextBlock>("day_night_igt")?.SetText($"IGT: {Tracker.InGameTime:h':'mm':'ss}");
            this.UpdateCounts();
        }

        public override void Expand()
        {
            base.Expand();
            //this.Refresh();
        }

        private void UpdateCounts()
        {

            //    progress += $"    -    {Tracker.GetFullIgt()} IGT";
            //string timeText = $"- 2:24:34";
            //this.estimatedTime.SetText(timeText);
            //ObtainedGodApple
            // bool godApple = Tracker.State.ObtainedGodApple;
            Console.WriteLine(Tracker.State.CriterionCompleted(AT, BJH));

            // Example data:
            int biomesCurrent = 10;
            int biomesTotal   = 36;
            int exploringCurrent = 5;
            int exploringTotal   = 24;
            int mobsCurrent = 12;
            int mobsTotal   = 23;
            int miscCurrent = 7;
            int miscTotal   = 17;
            int pointsCurrent = biomesCurrent + exploringCurrent + mobsCurrent + miscCurrent;
            int pointsTotal   = biomesTotal + exploringTotal + mobsTotal + miscTotal;

            double totalPoints = PointCriteria
                .Where(kvp => !RemainingCriteria.ContainsKey(kvp.Key))
                .Sum(kvp => kvp.Value);

            // update text
            string labelText = $"Est. Progress: {totalPoints}/{pointsTotal}";
            this.pointsLabel.SetText(labelText);

            // update the bar progress (0 to 1)
            float ratio = pointsTotal == 0 ? 0f : (float)pointsCurrent / pointsTotal;
            this.pointsBar.StartLerpToValue(ratio);

            string timeText = $"{!RemainingCriteria.ContainsKey(BP) ? 5 : 0}";
            this.estimatedTime.SetText(timeText);

            //tnt
            int tntCount = Tracker.State.TimesPickedUp(Tnt)
                + Tracker.State.TimesCrafted(Tnt)
                - Tracker.State.TimesUsed(Tnt)
                - Tracker.State.TimesDropped(Tnt);
            this.tnt?.SetText(Math.Max(0, tntCount).ToString());

            //gold
            int goldCount = GoldBlocks.GetPreciseEstimate(Tracker.State);
            this.gold?.SetText(goldCount.ToString());

            //obsidian
            int obsidianCount = Tracker.State.TimesPickedUp(Obsidian)
                - Tracker.State.TimesUsed(Obsidian)
                - Tracker.State.TimesDropped(Obsidian);
            this.obsidian?.SetText(Math.Max(0, obsidianCount).ToString());

            //shells
            int shellCount = Tracker.State.TimesPickedUp(Shell)
                - Tracker.State.TimesDropped(Shell);
            this.shells?.SetText($"{Math.Max(0, shellCount)}/8");

            //debris
            int debrisCount = Tracker.State.TimesPickedUp(Debris)
                - Tracker.State.TimesDropped(Debris);
            this.debris?.SetText(Math.Max(0, debrisCount).ToString());

            //beehives
            int beehiveCount = Tracker.State.TimesPickedUp(Beehive)
                - Tracker.State.TimesDropped(Beehive)
                - Tracker.State.TimesUsed(Beehive);
            this.beehives?.SetText(Math.Max(0, beehiveCount).ToString());

            //skulls
            int skullCount = Tracker.State.TimesPickedUp(Skull)
                - Tracker.State.TimesDropped(Skull)
                - Tracker.State.TimesUsed(Skull);
                - Tracker.State. (Skull);
            this.skulls?.SetText($"{Math.Max(0, skullCount)}/3");

            // int poisonCount = Tracker.State.TimesUsed(Poison);

            // string timeText = $"{poisonCount}";
            // this.estimatedTime.SetText(timeText);
        }
    }
}
