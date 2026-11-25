namespace Cr.Maui.Controls;

[ContentProperty(nameof(Columns))]
public partial class Table : ContentView
{
    public static readonly BindableProperty ColumnSpacingProperty =
        BindableProperty.Create(
            propertyName: nameof(ColumnSpacing),
            returnType: typeof(double),
            declaringType: typeof(Table),
            defaultValue: 0d,
            propertyChanged: ScheduleUpdate);

    public static readonly BindableProperty ColumnDefinitionsProperty =
        BindableProperty.Create(
            propertyName: nameof(ColumnDefinitions),
            returnType: typeof(ColumnDefinitionCollection),
            declaringType: typeof(Table),
            defaultValueCreator: _ => new ColumnDefinitionCollection(),
            propertyChanged: ScheduleUpdate);

    public static readonly BindableProperty RowPaddingProperty =
        BindableProperty.Create(
            propertyName: nameof(RowPadding),
            returnType: typeof(Thickness),
            declaringType: typeof(Table),
            defaultValue: Thickness.Zero,
            propertyChanged: ScheduleUpdate);

    public static readonly BindableProperty RowWrapperTemplateProperty =
        BindableProperty.Create(
            propertyName: nameof(RowWrapperTemplate),
            returnType: typeof(ControlTemplate),
            declaringType: typeof(Table),
            propertyChanged: ScheduleUpdate);

    public static readonly BindableProperty HeaderPaddingProperty =
        BindableProperty.Create(
            propertyName: nameof(HeaderPadding),
            returnType: typeof(Thickness),
            declaringType: typeof(Table),
            defaultValue: Thickness.Zero,
            propertyChanged: ScheduleUpdate);

    private readonly Grid _rootGrid = new() { RowDefinitions = [new(GridLength.Auto), new(GridLength.Star)] };
    private readonly Grid _headerGrid = new() { RowDefinitions = [new(GridLength.Star)] };

    private bool _updateScheduled;

    public Table()
    {
        Content = _rootGrid;
        _rootGrid.Add(_headerGrid, column: 0, row: 0);
        Columns.CollectionChanged += (_, _) => ScheduleUpdate();
    }

    public double ColumnSpacing
    {
        get => (double)GetValue(ColumnSpacingProperty);
        set => SetValue(ColumnSpacingProperty, value);
    }

    [TypeConverter(typeof(ColumnDefinitionCollectionTypeConverter))]
    public ColumnDefinitionCollection ColumnDefinitions
    {
        get => (ColumnDefinitionCollection)GetValue(ColumnDefinitionsProperty);
        set => SetValue(ColumnDefinitionsProperty, value);
    }

    public Thickness RowPadding
    {
        get => (Thickness)GetValue(RowPaddingProperty);
        set => SetValue(RowPaddingProperty, value);
    }

    public ControlTemplate? RowWrapperTemplate
    {
        get => (ControlTemplate?)GetValue(RowWrapperTemplateProperty);
        set => SetValue(RowWrapperTemplateProperty, value);
    }

    public Thickness HeaderPadding
    {
        get => (Thickness)GetValue(HeaderPaddingProperty);
        set => SetValue(HeaderPaddingProperty, value);
    }

    public ObservableCollection<TableColumn> Columns { get; } = [];

    public CollectionView? CollectionView
    {
        get;
        set
        {
            if (field == value)
                return;

            if (field is not null)
            {
                field.ItemTemplate = null;
                _rootGrid.Remove(field);
            }

            field = value;
            UpdateCollectionViewItemTemplate(field);
            AttachBodyView();
        }
    }

    public RefreshView? RefreshView
    {
        get;
        set
        {
            if (field == value)
                return;

            if (field is not null)
            {
                field.Content = null;
                _rootGrid.Remove(field);
            }

            field = value;
            AttachBodyView();
        }
    }

    private static void ScheduleUpdate(BindableObject bindable, object oldValue, object newValue)
    {
        ((Table)bindable).ScheduleUpdate();
    }

    private bool ScheduleUpdate()
    {
        if (_updateScheduled)
            return false;

        _updateScheduled = true;

        if (Dispatcher.Dispatch(ExecuteScheduledUpdate))
            return true;

        _updateScheduled = false;
        return false;
    }

    private void ExecuteScheduledUpdate()
    {
        _updateScheduled = false;
        PopulateHeaderGrid(_headerGrid);
        UpdateCollectionViewItemTemplate(CollectionView);
    }

    private void AttachBodyView()
    {
        IView? view;

        if (RefreshView is not null)
        {
            if (CollectionView is null)
            {
                RefreshView.Content = null;
                _rootGrid.Remove(RefreshView);
                return;
            }

            _rootGrid.Remove(CollectionView);
            RefreshView.Content = CollectionView;
            view = RefreshView;
        }
        else
        {
            view = CollectionView;
        }

        if (view is not null && !_rootGrid.Contains(view))
            _rootGrid.Add(view, column: 0, row: 1);
    }

    private IView CreateRow()
    {
        var grid = new Grid() { Padding = RowPadding };

        SyncGridColumnLayout(grid);

        for (var i = 0; i < Columns.Count; i++)
        {
            if (Columns[i].BodyTemplate?.CreateContent() is IView view)
            {
                grid.Add(view, column: i, row: 0);
            }
        }

        if (RowWrapperTemplate is null)
            return grid;

        return new ContentView
        {
            Content = grid,
            ControlTemplate = RowWrapperTemplate
        };
    }

    private void PopulateHeaderGrid(Grid grid)
    {
        grid.Clear();
        grid.Padding = HeaderPadding;

        SyncGridColumnLayout(grid);

        for (var i = 0; i < Columns.Count; i++)
        {
            var view = Columns[i].Header;

            if (view is not null)
            {
                grid.Add(view, column: i, row: 0);
            }
        }
    }

    private void SyncGridColumnLayout(Grid grid)
    {
        grid.ColumnDefinitions.Clear();
        grid.ColumnSpacing = ColumnSpacing;

        foreach (var columnDef in ColumnDefinitions)
        {
            grid.ColumnDefinitions.Add(new(columnDef.Width));
        }
    }

    private void UpdateCollectionViewItemTemplate(CollectionView? collectionView)
    {
        collectionView?.ItemTemplate = new(CreateRow);
    }
}