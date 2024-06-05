using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Views.Base
{
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnProperyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Устанавливает значение поля, при этом разрешая проблему кольцевых изменений
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">Поле, в которое происходит запись</param>
        /// <param name="value">Новое значение, которое будет записано в поле (при успехе, см. ниже)</param>
        /// <param name="propertyName">имя свойства. Можно не указывать. P.S. да, я ленив =)</param>
        /// <returns>True, если значение успешно изменено. False, если данные в поле уже совпадают с тем, что мы хотим туда записать</returns>
        protected virtual bool SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnProperyChanged(propertyName);

            return true;
        }
    }
}
