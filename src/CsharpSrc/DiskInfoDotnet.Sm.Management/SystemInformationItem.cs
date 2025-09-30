namespace DiskInfoDotnet.Sm.Management;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SystemInformationItem : INotifyPropertyChanged
{
    private string? field;

    private string? subField;

    private string? value;

    public string? Field
    {
        get
        {
            return field;
        }
        set
        {
            field = value;
            NotifyPropertyChanged("Field");
        }
    }

    public string? SubField
    {
        get
        {
            return subField;
        }
        set
        {
            subField = value;
            NotifyPropertyChanged("SubField");
        }
    }

    public string? Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
            NotifyPropertyChanged("Value");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void NotifyPropertyChanged(string propertyName)
    {
        if (this.PropertyChanged != null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public virtual List<string> toStringList()
    {
        if (Field is not null && SubField is not null && Value is not null)
        {
            return new List<string> { Field, SubField, Value };
        }
        throw new HelperClass.GettingExceptions($"Getting exception at {nameof(SystemInformationItem)}");

    }
}
