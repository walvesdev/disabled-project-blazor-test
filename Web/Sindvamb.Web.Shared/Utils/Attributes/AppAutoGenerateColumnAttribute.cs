namespace Sindvamb.Web.Shared.Utils.Attributes
{

    /// <summary>
    /// AutoGenerateColumn 标签类，用于 <see cref="Table{TItem}"/> 标识自动生成列
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AppAutoGenerateColumnAttribute : AutoGenerateBaseAttribute, IAppTableColumn
    {
        /// <summary>
        /// 获得/设置 显示顺序 ，规则如下：
        /// <para></para>
        /// &gt;0时排前面，1,2,3...
        /// <para></para>
        /// =0时排中间(默认)
        /// <para></para>
        /// &lt;0时排后面，...-3,-2,-1
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 获得/设置 是否为默认排序列 默认为 false
        /// </summary>
        public bool DefaultSort { get; set; }

        /// <summary>
        /// 获得/设置 是否不进行验证 默认为 false
        /// </summary>
        public bool SkipValidate { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsReadonlyWhenAdd { get; set; }

        bool? IAppTableColumn.IsReadonlyWhenAdd
        {
            get => IsReadonlyWhenAdd;
            set => IsReadonlyWhenAdd = value ?? false;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsReadonlyWhenEdit { get; set; }

        bool? IAppTableColumn.IsReadonlyWhenEdit
        {
            get => IsReadonlyWhenEdit;
            set => IsReadonlyWhenEdit = value ?? false;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsVisibleWhenAdd { get; set; } = true;

        bool? IAppTableColumn.IsVisibleWhenAdd
        {
            get => IsVisibleWhenAdd;
            set => IsVisibleWhenAdd = value ?? true;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsVisibleWhenEdit { get; set; } = true;

        /// <summary>
        /// 自定义搜索方法
        /// </summary>
        Func<IAppTableColumn, string?, SearchFilterAction>? IAppTableColumn.CustomSearch { get; set; }

        bool? IAppTableColumn.IsVisibleWhenEdit
        {
            get => IsVisibleWhenEdit;
            set => IsVisibleWhenEdit = value ?? true;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool? IsRequiredWhenAdd { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool? IsRequiredWhenEdit { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string RequiredErrorMessage { get; set; }

        /// <summary>
        /// 获得/设置 是否显示标签 Tooltip 多用于标签文字过长导致裁减时使用 默认 false
        /// </summary>
        public bool ShowLabelTooltip { get; set; }

        bool? IAppEditorItem.ShowLabelTooltip
        {
            get => ShowLabelTooltip;
            set => ShowLabelTooltip = value.HasValue && value.Value;
        }

        /// <summary>
        /// 获得/设置 是否为默认排序规则 默认为 SortOrder.Unset
        /// </summary>
        public SortOrder DefaultSortOrder { get; set; }

        IEnumerable<SelectedItem>? IAppEditorItem.Items { get; set; }

        /// <summary>
        /// 获得/设置 列宽
        /// </summary>
        public int Width { get; set; }

        int? IAppTableColumn.Width
        {
            get => Width <= 0 ? null : Width;
            set => Width = value ?? 0;
        }

        /// <summary>
        /// 获得/设置 是否固定本列 默认 false 不固定
        /// </summary>
        public bool Fixed { get; set; }

        /// <summary>
        /// 获得/设置 列 td 自定义样式 默认为 null 未设置
        /// </summary>
        public string? CssClass { get; set; }

        /// <summary>
        /// 获得/设置 显示节点阈值 默认值 BreakPoint.None 未设置
        /// </summary>
        public BreakPoint ShownWithBreakPoint { get; set; }

        /// <summary>
        /// 获得/设置 格式化字符串 如时间类型设置 yyyy-MM-dd
        /// </summary>
        public string? FormatString { get; set; }

        /// <summary>
        /// 获得/设置 placeholder 文本 默认为 null
        /// </summary>
        public string? PlaceHolder { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Func<object, Task<string>> Formatter { get; set; } = default;

        /// <summary>
        /// 获得/设置 编辑模板
        /// </summary>
        RenderFragment<object>? IAppEditorItem.EditTemplate { get; set; }

        /// <summary>
        /// 获得/设置 表头模板
        /// </summary>
        RenderFragment<IAppTableColumn>? IAppTableColumn.HeaderTemplate { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        RenderFragment<IAppTableColumn>? IAppTableColumn.ToolboxTemplate { get; set; }

        /// <summary>
        /// 获得/设置 组件类型 默认为 null
        /// </summary>
        public Type? ComponentType { get; set; }

        /// <summary>
        /// 获得/设置 组件自定义类型参数集合 默认为 null
        /// </summary>
        IEnumerable<KeyValuePair<string, object>>? IAppEditorItem.ComponentParameters { get; set; }

        /// <summary>
        /// 获得/设置 显示模板
        /// </summary>
        RenderFragment<object>? IAppTableColumn.Template { get; set; }

        /// <summary>
        /// 获得/设置 搜索模板
        /// </summary>
        RenderFragment<object>? IAppTableColumn.SearchTemplate { get; set; }

        /// <summary>
        /// 获得/设置 过滤模板
        /// </summary>
        RenderFragment? IAppTableColumn.FilterTemplate { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        Func<object?, Task<string?>>? IAppTableColumn.GetTooltipTextCallback { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        bool? IAppTableColumn.Searchable { get => Searchable; set => Searchable = value ?? false; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        bool? IAppTableColumn.Filterable { get => Filterable; set => Filterable = value ?? false; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        bool? IAppTableColumn.Sortable { get => Sortable; set => Sortable = value ?? false; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        bool? IAppTableColumn.TextWrap { get => TextWrap; set => TextWrap = value ?? false; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        bool? IAppTableColumn.TextEllipsis { get => TextEllipsis; set => TextEllipsis = value ?? false; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        bool? IAppEditorItem.Ignore { get => Ignore; set => Ignore = value ?? false; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        bool? IAppEditorItem.Readonly { get => Readonly; set => Readonly = value ?? false; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        bool? IAppTableColumn.Visible { get => Visible; set => Visible = value ?? true; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        bool? IAppTableColumn.ShowTips { get => ShowTips; set => ShowTips = value ?? false; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        bool? IAppTableColumn.ShowCopyColumn { get => ShowCopyColumn; set => ShowCopyColumn = value ?? false; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        Alignment? IAppTableColumn.Align { get => Align; set => Align = value ?? Alignment.None; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string? Step { get; set; }

        /// <summary>
        /// 获得/设置 行数
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// 获得/设置 控件的占列数值范围 1-12
        /// </summary>
        public int Cols { get; set; }

        /// <summary>
        /// 获得/设置 列过滤器
        /// </summary>
        IFilter? IAppTableColumn.Filter { get; set; }

        /// <summary>
        /// 获得 属性类型
        /// </summary>
        [NotNull]
        public Type? PropertyType { get; internal set; }

        /// <summary>
        /// 获得/设置 当前属性显示文字 列头或者标签名称
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [NotNull]
        internal string? FieldName { get; set; }

        /// <summary>
        /// 获得/设置 字段数据源下拉框是否显示搜索栏 默认 false 不显示
        /// </summary>
        public bool ShowSearchWhenSelect { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsPopover { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        IEnumerable<SelectedItem>? ILookup.Lookup { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        ILookupService? ILookup.LookupService { get; set; }

        /// <summary>
        /// <inheritdoc/>>
        /// </summary>
        public string? LookupServiceKey { get; set; }

        /// <summary>
        /// <inheritdoc/>>
        /// </summary>
        public object? LookupServiceData { get; set; }

        /// <summary>
        /// <inheritdoc/>>
        /// </summary>
        public StringComparison LookupStringComparison { get; set; } = StringComparison.OrdinalIgnoreCase;

        /// <summary>
        /// 获得/设置 单元格回调方法
        /// </summary>
        Action<TableCellArgs>? IAppTableColumn.OnCellRender { get; set; }

        /// <summary>
        /// 获得/设置 自定义验证集合
        /// </summary>
        List<IValidator>? IAppEditorItem.ValidateRules { get; set; }

        /// <summary>
        /// 获取绑定字段显示名称方法
        /// </summary>
        /// <returns></returns>
        public virtual string GetDisplayName() => Text ?? "";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public string GetFieldName() => FieldName;

        /// <summary>
        /// 获得/设置 当前属性分组
        /// </summary>
        public string? GroupName { get; set; }

        /// <summary>
        /// 获得/设置 当前属性分组排序 默认 0
        /// </summary>
        public int GroupOrder { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool HeaderTextWrap { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool ShowHeaderTooltip { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string? HeaderTextTooltip { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool HeaderTextEllipsis { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsMarkupString { get; set; }
    }

    public interface IAppTableColumn : IAppEditorItem
    {
        /// <summary>
        /// 获得/设置 是否允许排序 默认为 null
        /// </summary>
        bool? Sortable { get; set; }

        /// <summary>
        /// 获得/设置 是否为默认排序列 默认为 false
        /// </summary>
        bool DefaultSort { get; set; }

        /// <summary>
        /// 获得/设置 默认排序规则 默认为 SortOrder.Unset
        /// </summary>
        SortOrder DefaultSortOrder { get; set; }

        /// <summary>
        /// 获得/设置 是否允许过滤数据 默认为 null
        /// </summary>
        bool? Filterable { get; set; }

        /// <summary>
        /// 获得/设置 是否参与搜索 默认为 null
        /// </summary>
        bool? Searchable { get; set; }

        /// <summary>
        /// 获得/设置 列宽
        /// </summary>
        int? Width { get; set; }

        /// <summary>
        /// 获得/设置 是否固定本列 默认 false 不固定
        /// </summary>
        bool Fixed { get; set; }

        /// <summary>
        /// 获得/设置 本列是否允许换行 默认为 null
        /// </summary>
        bool? TextWrap { get; set; }

        /// <summary>
        /// 获得/设置 本列文本超出省略 默认为 null
        /// </summary>
        bool? TextEllipsis { get; set; }

        /// <summary>
        /// 获得/设置 是否表头允许折行 默认 false 不折行
        /// </summary>
        bool HeaderTextWrap { get; set; }

        /// <summary>
        /// 获得/设置 是否表头显示 Tooltip 默认 false 不显示 可配合 <see cref="HeaderTextEllipsis"/> 使用 设置 <see cref="HeaderTextWrap"/> 为 true 时本参数不生效
        /// </summary>
        bool ShowHeaderTooltip { get; set; }

        /// <summary>
        /// 获得/设置 是否表头 Tooltip 内容
        /// </summary>
        string? HeaderTextTooltip { get; set; }

        /// <summary>
        /// 获得/设置 是否表头溢出时截断 默认 false 不截断 可配合 <see cref="HeaderTextTooltip"/> 使用 设置 <see cref="HeaderTextWrap"/> 为 true 时本参数不生效
        /// </summary>
        bool HeaderTextEllipsis { get; set; }

        /// <summary>
        /// 获得/设置 列 td 自定义样式 默认为 null 未设置
        /// </summary>
        string? CssClass { get; set; }

        /// <summary>
        /// 显示节点阈值 默认值 BreakPoint.None 未设置
        /// </summary>
        BreakPoint ShownWithBreakPoint { get; set; }

        /// <summary>
        /// 获得/设置 是否可以拷贝列 默认 null 不可以
        /// </summary>
        bool? ShowCopyColumn { get; set; }

        /// <summary>
        /// 获得/设置 显示模板
        /// </summary>
        RenderFragment<object>? Template { get; set; }

        /// <summary>
        /// 获得/设置 搜索模板
        /// </summary>
        RenderFragment<object>? SearchTemplate { get; set; }

        /// <summary>
        /// 获得/设置 过滤模板
        /// </summary>
        RenderFragment? FilterTemplate { get; set; }

        /// <summary>
        /// 获得/设置 表头模板
        /// </summary>
        RenderFragment<IAppTableColumn>? HeaderTemplate { get; set; }

        /// <summary>
        /// 获得/设置 工具栏模板 默认 null
        /// </summary>
        RenderFragment<IAppTableColumn>? ToolboxTemplate { get; set; }

        /// <summary>
        /// 获得/设置 列过滤器
        /// </summary>
        IFilter? Filter { get; set; }

        /// <summary>
        /// 获得/设置 格式化字符串 如时间类型设置 yyyy-MM-dd
        /// </summary>
        string? FormatString { get; set; }

        /// <summary>
        /// 获得/设置 列格式化回调委托 <see cref="TableColumnContext{TItem, TValue}"/>
        /// </summary>
        Func<object?, Task<string?>>? Formatter { get; set; }

        /// <summary>
        /// 获得/设置 文字对齐方式 默认为 null 使用 Alignment.None
        /// </summary>
        Alignment? Align { get; set; }

        /// <summary>
        /// 获得/设置 字段鼠标悬停提示 默认为 null 使用 false 值
        /// </summary>
        bool? ShowTips { get; set; }

        /// <summary>
        /// 获得/设置 鼠标悬停提示自定义内容回调委托 默认 null 使用当前值
        /// </summary>
        Func<object?, Task<string?>>? GetTooltipTextCallback { get; set; }

        /// <summary>
        /// 获得/设置 单元格回调方法
        /// </summary>
        Action<TableCellArgs>? OnCellRender { get; set; }

        /// <summary>
        /// 获得/设置 是否为 MarkupString 默认 false
        /// </summary>
        bool IsMarkupString { get; set; }

        /// <summary>
        /// 获得/设置 新建时是否为必填项 默认为 null
        /// </summary>
        bool? IsRequiredWhenAdd { get; set; }

        /// <summary>
        /// 获得/设置 编辑时是否为必填项 默认为 null
        /// </summary>
        bool? IsRequiredWhenEdit { get; set; }

        /// <summary>
        /// 获得/设置 新建时此列只读 默认为 null 使用 <see cref="IAppEditorItem.Readonly"/> 值
        /// </summary>
        bool? IsReadonlyWhenAdd { get; set; }

        /// <summary>
        /// 获得/设置 编辑时此列只读 默认为 null 使用 <see cref="IAppEditorItem.Readonly"/> 值
        /// </summary>
        bool? IsReadonlyWhenEdit { get; set; }

        /// <summary>
        /// 获得/设置 当前编辑项是否显示 默认为 null 未设置时为 true
        /// </summary>
        bool? Visible { get; set; }

        /// <summary>
        /// 获得/设置 新建时是否此列显示  默认为 null 使用 <see cref="Visible"/> 值
        /// </summary>
        bool? IsVisibleWhenAdd { get; set; }

        /// <summary>
        /// 获得/设置 编辑时是否此列显示  默认为 null 使用 <see cref="Visible"/> 值
        /// </summary>
        bool? IsVisibleWhenEdit { get; set; }

        /// <summary>
        /// 获得/设置 自定义搜索逻辑
        /// </summary>
        Func<IAppTableColumn, string?, SearchFilterAction>? CustomSearch { get; set; }
    }

    public interface IAppEditorItem : ILookup
    {
        /// <summary>
        /// 获得/设置 绑定列类型
        /// </summary>
        Type PropertyType { get; }

        /// <summary>
        /// 获得/设置 当前编辑项是否可编辑 默认为 true
        /// </summary>
        [Obsolete("已弃用，是否显示使用 Visible 参数，新建时使用 IsVisibleWhenAdd 编辑时使用 IsVisibleWhenEdit 只读使用 Readonly 参数，新建时使用 IsReadonlyWhenAdd 编辑时使用 IsReadonlyWhenEdit 参数; Deprecated use Visible parameter. IsVisibleWhenAdd should be used when creating a new one, and IsVisibleWhenEdit should be used when editing")]
        bool Editable { get; set; }

        /// <summary>
        /// 获得/设置 当前编辑项是否只读 默认为 false
        /// </summary>
        bool? Readonly { get; set; }

        /// <summary>
        /// 获得/设置 当前编辑项是否忽略 默认为 false 当设置为 true 时 UI 不生成此列
        /// </summary>
        bool? Ignore { get; set; }

        /// <summary>
        /// 获得/设置 是否不进行验证 默认为 false
        /// </summary>
        bool SkipValidate { get; set; }

        /// <summary>
        /// 获得/设置 表头显示文字
        /// </summary>
        string? Text { get; set; }

        /// <summary>
        /// 获得/设置 是否显示标签 Tooltip 多用于标签文字过长导致裁减时使用 默认 null
        /// </summary>
        bool? ShowLabelTooltip { get; set; }

        /// <summary>
        /// 获得/设置 placeholder 文本 默认为 null
        /// </summary>
        string? PlaceHolder { get; set; }

        /// <summary>
        /// 获得/设置 额外数据源一般用于 Select 或者 CheckboxList 这种需要额外配置数据源组件使用
        /// </summary>
        IEnumerable<SelectedItem>? Items { get; set; }

        /// <summary>
        /// 获得/设置 步长 默认为 null 设置 any 时忽略检查
        /// </summary>
        string? Step { get; set; }

        /// <summary>
        /// 获得/设置 Textarea 行数 默认为 0
        /// </summary>
        int Rows { get; set; }

        /// <summary>
        /// 获得/设置 编辑模板
        /// </summary>
        RenderFragment<object>? EditTemplate { get; set; }

        /// <summary>
        /// 获得/设置 组件类型 默认为 null
        /// </summary>
        Type ComponentType { get; set; }

        /// <summary>
        /// 获得/设置 组件自定义类型参数集合 默认为 null
        /// </summary>
        IEnumerable<KeyValuePair<string, object>>? ComponentParameters { get; set; }

        /// <summary>
        /// 获得/设置 字段数据源下拉框是否显示搜索栏 默认 false 不显示
        /// </summary>
        bool ShowSearchWhenSelect { get; set; }

        /// <summary>
        /// 获得/设置 是否使用 Popover 渲染下拉框 默认 false
        /// </summary>
        bool IsPopover { get; set; }

        /// <summary>
        /// 获得/设置 自定义验证集合
        /// </summary>
        List<IValidator>? ValidateRules { get; set; }

        /// <summary>
        /// 获取绑定字段显示名称方法
        /// </summary>
        string GetDisplayName();

        /// <summary>
        /// 获取绑定字段信息方法
        /// </summary>
        string GetFieldName();

        /// <summary>
        /// 获得/设置 顺序号
        /// </summary>
        int Order { get; set; }

        /// <summary>
        /// 获得/设置 当前属性分组
        /// </summary>
        string? GroupName { get; set; }

        /// <summary>
        /// 获得/设置 当前属性分组排序 默认 0
        /// </summary>
        int GroupOrder { get; set; }

        /// <summary>
        /// 获得/设置 是否为必填项 默认为 null
        /// </summary>
        bool Required { get; set; }

        /// <summary>
        /// 获得/设置 必填项缺失时错误提示文本 默认为 null
        /// </summary>
        string RequiredErrorMessage { get; set; }
    }
}
