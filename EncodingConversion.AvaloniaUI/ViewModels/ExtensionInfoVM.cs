using EncodingConversion.Logic;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncodingConversion.AvaloniaUI.ViewModels
{
    //internal class ExtensionInfoVM : ViewModelBase
    //{
    //    private readonly ExtensionInfo _model;

    //    private bool _isEnable;
    //    public bool isEnable
    //    {
    //        get { return _model.IsEnable; }
    //        set { this.RaiseAndSetIfChanged(ref _isEnable, value); }
    //    }

    //    private string _symbols;
    //    public string Symbols
    //    {
    //        get { return _model.Symbols; }
    //        set { this.RaiseAndSetIfChanged(ref _symbols, value); }
    //    }

    //    public ExtensionInfoVM(ExtensionInfo model)
    //    {
    //        _model = model;

    //        this.WhenAnyValue(x => x.isEnable)
    //            .Subscribe(isEnabled => _model.IsEnable = isEnabled);
    //        this.WhenAnyValue(x => x.Symbols)
    //            .Subscribe(symbols => _model.Symbols = symbols);
    //    }
    //}
}
