using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using SpaceEngineers2D.Controllers;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Model.Blocks;
using SpaceEngineers2D.Model.Entities;
using SpaceEngineers2D.Model.Items;
using SpaceEngineers2D.Persistence;
using SpaceEngineers2D.Physics;
using SpaceEngineers2D.View.Inventory;

namespace SpaceEngineers2D.View
{
    public class ApplicationViewModel : PropertyObservable
    {
        private readonly Random _random = new Random();
        private object _interactingBlockViewModel;
        private bool _isMainMenuActive;
        private World _world;
        private WorldController _worldController;
        private InventoryViewModel _playerInventoryViewModel;
        private string _currentSaveName;
        private bool _isLoadGameViewActive;
        private bool _isNewGameViewActive;
        private string _newGameName;
        private IReadOnlyList<string> _savedGames;
        private Player _player;
        private PhysicsEngine _physics;

        public BlockTypes BlockTypes { get; }

        public EntityTypes EntityTypes { get; }

        public ItemTypes ItemTypes { get; }

        public World World
        {
            get => _world;
            private set => SetProperty(ref _world, value);
        }

        public Player Player
        {
            get => _player;
            private set => SetProperty(ref _player, value);
        }

        public PhysicsEngine Physics
        {
            get => _physics;
            private set => SetProperty(ref _physics, value);
        }

        public WorldController WorldController
        {
            get => _worldController;
            set => SetProperty(ref _worldController, value);
        }

        public InventoryViewModel PlayerInventoryViewModel
        {
            get => _playerInventoryViewModel;
            private set => SetProperty(ref _playerInventoryViewModel, value);
        }

        public object InteractingBlockViewModel
        {
            get => _interactingBlockViewModel;
            private set => SetProperty(ref _interactingBlockViewModel, value);
        }

        public bool IsMainMenuActive
        {
            get => _isMainMenuActive;
            private set => SetProperty(ref _isMainMenuActive, value);
        }

        public bool IsLoadGameViewActive
        {
            get => _isLoadGameViewActive;
            private set => SetProperty(ref _isLoadGameViewActive, value);
        }

        public IReadOnlyList<string> SavedGames
        {
            get => _savedGames;
            private set => SetProperty(ref _savedGames, value);
        }

        public string CurrentSaveName
        {
            get => _currentSaveName;
            private set => SetProperty(ref _currentSaveName, value);
        }

        public bool IsNewGameViewActive
        {
            get => _isNewGameViewActive;
            private set => SetProperty(ref _isNewGameViewActive, value);
        }

        public string NewGameName
        {
            get => _newGameName;
            set
            {
                if (SetProperty(ref _newGameName, value))
                {
                    // TODO: Find out why CanExecute is not called automatically.
                    CreateNewGameCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public Command ToggleMainMenuCommand { get; }
        public Command ActivateLoadGameViewCommand { get; }
        public Command<string> LoadGameCommand { get; }
        public Command CancelLoadGameCommand { get; }
        public Command ActivateNewGameViewCommand { get; }
        public Command CreateNewGameCommand { get; }
        public Command CancelNewGameCommand { get; }
        public Command SaveGameCommand { get; }

        private PersistenceService PersistenceService { get; }

        public ApplicationViewModel(PersistenceService persistenceService)
        {
            PersistenceService = persistenceService;

            ItemTypes = new ItemTypes();
            BlockTypes = new BlockTypes(ItemTypes);
            EntityTypes = new EntityTypes();

            ToggleMainMenuCommand = new Command(CanToggleMainMenu, ToggleMainMenu);
            ActivateLoadGameViewCommand = new Command(CanActivateLoadGameView, ActivateLoadGameView);
            LoadGameCommand = new Command<string>(CanLoadGame, LoadGame);
            CancelLoadGameCommand = new Command(CancelLoadGame);
            ActivateNewGameViewCommand = new Command(CanActivateeNewGameView, ActivateNewGameView);
            CreateNewGameCommand = new Command(CanCreateNewGame, CreateNewGame);
            CancelNewGameCommand = new Command(CancelNewGame);
            SaveGameCommand = new Command(CanSaveGame, SaveGame);

            ToggleMainMenu();
        }

        public bool CanToggleMainMenu()
        {
            if (IsMainMenuActive && World == null)
                return false;

            return true;
        }

        public void ToggleMainMenu()
        {
            if (IsMainMenuActive)
            {
                CloseMainMenu();
            }
            else
            {
                IsMainMenuActive = true;
            }
        }

        public void CloseMainMenu()
        {
            IsNewGameViewActive = false;
            IsLoadGameViewActive = false;
            IsMainMenuActive = false;
        }

        public bool CanActivateLoadGameView()
        {
            return true;
        }

        public void ActivateLoadGameView()
        {
            SavedGames = PersistenceService.Get();
            IsMainMenuActive = true;
            IsNewGameViewActive = false;
            IsLoadGameViewActive = true;
        }
        
        private bool CanActivateeNewGameView()
        {
            return true;
        }

        private void ActivateNewGameView()
        {
            IsMainMenuActive = true;
            IsLoadGameViewActive = false;
            IsNewGameViewActive = true;
        }

        public bool CanLoadGame(string name)
        {
            return true;
        }

        public void LoadGame(string name)
        {
            if (World != null)
                PersistenceService.Save(CurrentSaveName, World);

            SetWorld(null);

            var world = CreateWorld();

            PersistenceService.Load(name, world);

            Player = world.Entities.OfType<Player>().First();

            CurrentSaveName = name;

            SetWorld(world);

            CloseMainMenu();
        }

        private void CancelLoadGame()
        {
            IsLoadGameViewActive = false;
        }

        public bool CanCreateNewGame()
        {
            return !string.IsNullOrWhiteSpace(NewGameName)
                && !PersistenceService.Get()
                    .Select(name => name.ToUpper(CultureInfo.InvariantCulture))
                    .Contains(NewGameName.ToUpper(CultureInfo.InvariantCulture));
        }

        public void CreateNewGame()
        {
            if (World != null)
                PersistenceService.Save(CurrentSaveName, World);

            var world = CreateWorld();
            InitializeNewWorld(world);
            
            PersistenceService.Save(NewGameName, world);
            CurrentSaveName = NewGameName;
            
            SetWorld(world);

            NewGameName = string.Empty;
            CloseMainMenu();
        }

        private void CancelNewGame()
        {
            IsNewGameViewActive = false;
            NewGameName = string.Empty;
        }

        public bool CanSaveGame()
        {
            return World != null;
        }

        public void SaveGame()
        {
            PersistenceService.Save(CurrentSaveName, World);
            CloseMainMenu();
        }

        private World CreateWorld()
        {
            var world = new World(BlockTypes, EntityTypes, ItemTypes, new Camera { Zoom = 0.05f }, 30 * Constants.BlockSize);
            world.Grids.Add(new Grid(0, world.CoordinateSystem));

            return world;
        }

        private void InitializeNewWorld(World world)
        {
            InitializeWorldEnvironment(world);

            Player = EntityTypes.Player.Instantiate(BlockTypes);
            Player.Position = new IntVector(0, -6, 0) * Constants.BlockSize;
            world.Entities.Add(Player);

            var frog = EntityTypes.Frog.Instantiate();
            frog.Position = new IntVector(0, Player.Bounds.Bottom, 0);
            world.Entities.Add(frog);

            /* for (var i = 0; i < 3; i++)
            {
                var frog = EntityTypes.Frog.Instantiate();
                frog.Position = new IntVector(_random.Next(0, world.Width), -5 * Constants.BlockSize, 0);
                world.Entities.Add(frog);
            } */
        }

        private void InitializeWorldEnvironment(World world)
        {
            var grid = world.Grids.First();

            for (var x = 0; x < world.Width / Constants.BlockSize; x++)
            {
                for (var y = 0; y < 30; y++)
                {
                    if (y == 0)
                    {
                        if (_random.Next(0, 100) < 80)
                            grid.SetBlock(new IntVector(x, y, 0) * Constants.BlockSize,
                                world.BlockTypes.Grass.InstantiateBlock());

                        continue;
                    }

                    if (y == 1)
                    {
                        grid.SetBlock(new IntVector(x, y, 0) * Constants.BlockSize,
                            world.BlockTypes.DirtWithGrass.Instantiate());
                        continue;
                    }

                    if (y == 2 || (y == 3 && _random.Next(0, 100) < 50))
                    {
                        grid.SetBlock(new IntVector(x, y, 0) * Constants.BlockSize, world.BlockTypes.Dirt.Instantiate());
                        continue;
                    }

                    if (y == 4 && _random.Next(0, 100) < 50)
                    {
                        var topBlock = grid.GetBlock(new IntVector(x, y - 1, 0) * Constants.BlockSize);
                        if (topBlock.Object.BlockType == world.BlockTypes.Dirt)
                        {
                            grid.SetBlock(new IntVector(x, y, 0) * Constants.BlockSize, world.BlockTypes.Dirt.Instantiate());
                            continue;
                        }
                    }

                    if (_random.Next(0, 100) < 5)
                    {
                        grid.SetBlock(new IntVector(x, y, 0) * Constants.BlockSize,
                            world.BlockTypes.IronOreDeposit.Instantiate());
                        continue;
                    }

                    if (_random.Next(0, 100) < 5)
                    {
                        grid.SetBlock(new IntVector(x, y, 0) * Constants.BlockSize, world.BlockTypes.CoalDeposit.Instantiate());
                        continue;
                    }

                    grid.SetBlock(new IntVector(x, y, 0) * Constants.BlockSize, world.BlockTypes.Rock.Instantiate());
                }
            }

            for (var x = 0; x < world.Width / Constants.BlockSize; x++)
            {
                if (_random.Next(0, 100) > 10)
                    continue;

                var height = _random.Next(2, 4);

                for (var y = -height; y <= 0; y++)
                {
                    grid.SetBlock(new IntVector(x, y, 0) * Constants.BlockSize, world.BlockTypes.Reed.InstantiateBlock());
                }
            }

            var blastFurnace = world.BlockTypes.BlastFurnace.Instantiate();
            blastFurnace.BlueprintState.FinishImmediately();
            grid.SetBlock(new IntVector(3, 0, 0) * Constants.BlockSize, blastFurnace);
        }

        private void SetWorld(World world)
        {
            if (World != null)
            {
                Player.PropertyChanged -= OnPlayerPropertyChanged;
            }

            World = world;

            if (world != null)
            {
                PlayerInventoryViewModel = new InventoryViewModel(Player.Inventory, Player.HandInventorySlot);
                Physics = new PhysicsEngine(world.CoordinateSystem);
                WorldController = new WorldController(world);

                Physics.Initialize(world);

                Player.PropertyChanged += OnPlayerPropertyChanged;
            }
            else
            {
                PlayerInventoryViewModel = null;
                WorldController = null;
            }

            UpdateInteractingBlockViewModel();

            // TODO: Find out why CanExecute is not called automatically.
            SaveGameCommand.RaiseCanExecuteChanged();
        }

        private void OnPlayerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Player.InteractingBlock))
            {
                UpdateInteractingBlockViewModel();
            }
        }

        private void UpdateInteractingBlockViewModel()
        {
            switch (Player?.InteractingBlock?.Object)
            {
                case BlastFurnaceBlock blastFurnaceBlock:
                    InteractingBlockViewModel = new BlastFurnaceViewModel(new InventoryViewModel(blastFurnaceBlock.Inventory, Player.HandInventorySlot));
                    break;
                default:
                    InteractingBlockViewModel = null;
                    break;
            }
        }
    }
}

