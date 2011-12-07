using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace BeaverMusic.UI.Shell
{
    [Export]
    internal class AlbumListViewModel : BindableBase
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IViewFactory _viewFactory;

        [ImportingConstructor]
        public AlbumListViewModel(IAlbumRepository albumRepository, IViewFactory viewFactory)
        {
            _albumRepository = albumRepository;
            _viewFactory = viewFactory;
            Albums = albumRepository.GetAlbums();

            InitializeCommands();
        }

        public IEnumerable<Album> Albums { get; private set; }

        private Album _selectedAlbum;
        public Album SelectedAlbum
        {
            get { return _selectedAlbum; }
            set { SetProperty(ref _selectedAlbum, value, "SelectedAlbum"); }
        }

        #region Commands

        public ICommand NewAlbumCommand { get; private set; }
        public ICommand DeleteAlbumCommand { get; private set; }
        public ICommand EditAlbumCommand { get; private set; }

        private void InitializeCommands()
        {
            NewAlbumCommand = new DelegateCommand(NewAlbum);
            DeleteAlbumCommand = new DelegateCommand(DeleteSelectedAlbum);
            EditAlbumCommand = new DelegateCommand(EditSelectedAlbum);
        }

        #endregion

        public void NewAlbum()
        {
            EditAlbum(new Album());
        }

        public void DeleteSelectedAlbum()
        {
            if (_selectedAlbum != null)
                _albumRepository.RemoveAlbum(_selectedAlbum.Id.Value);
        }

        public void EditSelectedAlbum()
        {
            if (_selectedAlbum != null)
                EditAlbum(_selectedAlbum);
        }

        private void EditAlbum(Album album)
        {
            dynamic view = null;
            AlbumEditDialogViewModel viewModel = null;
            _viewFactory.CreateView<AlbumEditDialogView, AlbumEditDialogViewModel>(ref view, ref viewModel);

            viewModel.Initialize(album);

            view.Show();
            viewModel.Completed += (s, e) => view.Close();
        }
    }
}
