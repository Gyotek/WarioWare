//Copyright (c) Ewan Argouse - http://narudgi.github.io/

using Sirenix.OdinInspector;
using UnityAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
	[System.Serializable]
	public class FloatUnityEvent : UnityEvent<float> { }

	public class HealthSystem : MonoBehaviour
	{
		[SerializeField] private FloatVariable healthAtom = default; 
		
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
		[ShowInInspector, ReadOnly] private float currentHealth = default;

		private UnityAction<float> healthAtomSettingCall;
		public UnityAction<float> HealthAtomSettingCall => healthAtomSettingCall;
			
		private void Awake()
		{
			ResetMaxHealth();
			ResetLife();
		}

		public void SubscribeHealthAtom()
		{
			if (healthAtomSettingCall != null) return;
			
			healthAtomSettingCall = new UnityAction<float>(val => healthAtom.SetValue(currentHealth));
			OnHealthChanged.AddListener(healthAtomSettingCall);
		}
		public void UnsubscribeHealthAtom()
		{
			if (healthAtomSettingCall == null) return;
			
			OnHealthChanged.RemoveListener(healthAtomSettingCall);
			healthAtomSettingCall = null;
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