﻿using SadConsole;
using System;
using System.Linq;
using RogueGame.GameSystems.Items;
using RogueGame.Maps;
using RogueGame.Ui;

namespace RogueGame
{
    internal sealed class RogueGame : IDisposable
    {
        private readonly UiManager _uiManager;

        private bool disposedValue;

        public RogueGame()
        {
            _uiManager = new UiManager();
        }

        public void Run()
        {
            Game.Create(_uiManager.ViewPortWidth, _uiManager.ViewPortHeight);
            Game.OnInitialize = Init;

            // Start the game.
            Game.Instance.Run();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Init()
        {
            Global.LoadFont(UiManager.TileSetFontPath);

            InitColors();

            // gonna put these in a game manager
            var itemLoader = new ItemTemplateLoader();
            var items = itemLoader.Load();

            var mapLoader = new MapTemplateLoader();
            var mapTemplate = mapLoader.Load();
            var mapPlanFactory = new MapPlanFactory();
            var maps = mapTemplate.ToDictionary(t => t.Key, t => mapPlanFactory.Create(t.Value, items));
            
            //Global.CurrentScreen = _uiManager.CreateMainMenu();
            Global.CurrentScreen = _uiManager.CreateMapScreen(maps["MAP_TESTAREA"]);
        }

        private void InitColors()
        {
            var colors = SadConsole.Themes.Library.Default.Colors;

            colors.TitleText = colors.Orange;

            colors.TextBright = colors.White;
            colors.Text = colors.Blue;
            colors.TextSelected = colors.White;
            colors.TextSelectedDark = colors.White;
            colors.TextLight = colors.GrayDark;
            colors.TextDark = colors.Green;
            colors.TextFocused = colors.Cyan;

            colors.Lines = colors.Gray;

            colors.ControlBack = ColorHelper.MidnightEstBlue;
            colors.ControlBackLight = ColorHelper.MidnightEstBlue;
            colors.ControlBackSelected = colors.GrayDark;
            colors.ControlBackDark = ColorHelper.MidnightEstBlue;
            colors.ControlHostBack = ColorHelper.MidnightEstBlue;
            colors.ControlHostFore = colors.Text;

            colors.RebuildAppearances();
        }

        private void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                Game.Instance.Dispose();
            }

            disposedValue = true;
        }
    }
}
