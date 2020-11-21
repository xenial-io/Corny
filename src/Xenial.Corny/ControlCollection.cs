using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Xenial.Delicious.Corny
{
    /// <summary>
    /// Based on http://stackoverflow.com/questions/224155/when-clearing-an-observablecollection-there-are-no-items-in-e-olditems
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ControlCollection<T> : ObservableCollection<T>
    {
        //Flag used to prevent OnCollectionChanged from firing during a bulk operation like Add(IEnumerable<T>) and Clear()
        private bool suppressCollectionChanged;

        /// Overridden so that we may manually call registered handlers and differentiate between those that do and don't require Action.Reset args.
        public override event NotifyCollectionChangedEventHandler? CollectionChanged;

        public ControlCollection() : base() { }

        public ControlCollection(IEnumerable<T> data) : base(data) { }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!suppressCollectionChanged)
            {
                base.OnCollectionChanged(e);

                CollectionChanged?.Invoke(this, e);
            }
        }

        //CollectionViews raise an error when they are passed a NotifyCollectionChangedEventArgs that indicates more than
        //one element has been added or removed. They prefer to receive a "Action=Reset" notification, but this is not suitable
        //for applications in code, so we actually check the type we're notifying on and pass a customized event args.
        protected virtual void OnCollectionChangedMultiItem(NotifyCollectionChangedEventArgs e)
        {
            var handlers = CollectionChanged;
            if (handlers != null)
            {
                foreach (var handler in handlers.GetInvocationList().OfType<NotifyCollectionChangedEventHandler>())
                {
                    handler(this, e);
                }
            }
        }

        protected override void ClearItems()
        {
            if (Count == 0) { return; }

            var removed = new List<T>(this);

            suppressCollectionChanged = true;

            base.ClearItems();

            suppressCollectionChanged = false;

            OnCollectionChangedMultiItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed));
        }

        public void Add(IEnumerable<T> toAdd)
        {
            var _ = toAdd ?? throw new ArgumentNullException(nameof(toAdd));

            if (this == toAdd)
            {
                throw new InvalidOperationException("Invalid operation. This would result in iterating over a collection as it is being modified.");
            }

            suppressCollectionChanged = true;

            foreach (var item in toAdd)
            {
                Add(item);
            }

            suppressCollectionChanged = false;

            OnCollectionChangedMultiItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T>(toAdd)));
        }

        public void Remove(IEnumerable<T> toRemove)
        {
            var _ = toRemove ?? throw new ArgumentNullException(nameof(toRemove));

            if (this == toRemove)
            {
                throw new InvalidOperationException("Invalid operation. This would result in iterating over a collection as it is being modified.");
            }

            suppressCollectionChanged = true;

            foreach (var item in toRemove)
            {
                Remove(item);
            }

            suppressCollectionChanged = false;

            OnCollectionChangedMultiItem(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T>(toRemove)));
        }
    }
}
