//Copyright (c) Ewan Argouse - http://narudgi.github.io/

using UnityEngine;
using UnityEngine.Events;

namespace Game
{
	[System.Serializable]
	public class FloatUnityEvent : UnityEvent<float> { }

	public class Health : MonoBehaviour
	{
		public bool IsAlive { get; set; } = true;
		public float MaxHealth => maxHealth;

		public float CurrentHealth
		{
			get => currentHealth;
			private set
			{
				currentHealth = value;
				OnHealthChanged?.Invoke(currentHealth);
			}
		}

		[SerializeField] private float maxHealth = default;

		public FloatUnityEvent OnMaxHealthChanged = default;
		public FloatUnityEvent OnHealthChanged = default;
		public UnityEvent OnHurt = default;
		public UnityEvent OnDeath = default;

		private float currentMaxHealth = default;
		private float currentHealth = default;

		public Health() { }

		private void Awake()
		{
			ResetMaxHealth();
			ResetLife();
		}

		public void SetMaxHealth(float value)
		{
			currentMaxHealth = value;
			OnMaxHealthChanged?.Invoke(currentMaxHealth);
		}

		public void Hurt(float dmg)
		{
			if (!IsAlive) return;

			CurrentHealth -= dmg;

			OnHurt?.Invoke();

			if (CurrentHealth > currentMaxHealth)
				CurrentHealth = currentMaxHealth;

			if (CurrentHealth > 0f) return;

			CurrentHealth = 0f;
			IsAlive = false;

			OnDeath?.Invoke();
		}

		public void ResetLife()
		{
			IsAlive = true;
			CurrentHealth = currentMaxHealth;
		}

		public void ResetMaxHealth() => SetMaxHealth(maxHealth);

		public float GetHealthRatio()
		{
			if (currentMaxHealth <= 0) return 0;
			return CurrentHealth / currentMaxHealth;
		}

		public float HealthDifference() => maxHealth - CurrentHealth;
	}
}