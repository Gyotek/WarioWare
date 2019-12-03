// Copyright (c) - Ewan Argouse

namespace Game
{
	/// <summary>
	/// Allow to check change on update
	/// </summary>
	/// <typeparam name="T">Equatable type</typeparam>
	public struct ObserveUpdateChange<T>
	{
        public ObserveUpdateChange(T lastValue)
        {
            this.LastValue = lastValue;
            this.Changed = default;
        }

        public T LastValue { get; private set; }

        public delegate void OnChanged();
        public delegate void OnChanged<U>(U value);
        public OnChanged<T> Changed { get; private set; }

        /// <summary>
        /// Assign to event triggered on observe change
        /// </summary>
        /// <param name="m_current">Current value</param>
        public void Assign(OnChanged<T> onChanged) => Assign(default, onChanged);

        /// <summary>
        /// Assign to event triggered on observe change
        /// </summary>
        /// <param name="m_last">Last value</param>
        /// <param name="onChanged">Event to trigger</param>
        public void Assign(T lastValue, OnChanged<T> onChanged)
        {
            this.LastValue = lastValue;
            this.Changed = onChanged;
        }

        /// <summary>
        /// Observe if value has changed, and trigger it
        /// <para>Assign must be executed</para> 
        /// </summary>
        /// <param name="value">Value to observe</param>
        public void Observe(T value) => Observe(value, Changed);

		/// <summary>
		/// Observe if value has changed, and trigger it
		/// </summary>
		/// <param name="value">Value to observe</param>
		/// <param name="onChanged">Triggered if changed</param>
		public void Observe(T value, OnChanged<T> onChanged)
		{
			if (value != null && !value.Equals(LastValue))
				onChanged?.Invoke(value);

            LastValue = value;
		}
	}
}