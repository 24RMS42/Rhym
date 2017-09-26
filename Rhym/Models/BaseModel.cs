using System;
using System.ComponentModel;

namespace Rhym
{
    public class BaseModel : INotifyPropertyChanged
    {
    	public event PropertyChangedEventHandler PropertyChanged;

    	protected virtual void OnPropertyChanged(string propertyName)
    	{
    		if (PropertyChanged != null)
    		{
    			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    		}
        }
    }
}
