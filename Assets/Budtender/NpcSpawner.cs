using Budtender.Orders;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using Budtender.Traits;
using System.Linq;
using System;
using Budtender;

namespace Budtender.Shop
{
    public class NpcSpawner : MonoBehaviour
    {
        public NpcSpawnerTemplate template; //debug for now;

        [SerializeField]
        public List<Customer> Customers = new List<Customer>();

        [ShowInInspector, ReadOnly]
        public int CurrentCustomerIndex { get; private set; } = -1;

        [ShowInInspector, ReadOnly]
        public bool IsSellingDay => CurrentCustomerIndex >= 0 && CurrentCustomerIndex < Customers.Count;

        [ShowInInspector, ReadOnly]
        public Customer CurrentCustomer => IsSellingDay ? Customers[CurrentCustomerIndex] : null;

        [ShowInInspector, ReadOnly, TextArea(3, 10)]
        public string CurrentOrderSummary => CurrentCustomer?.OrderSummary() ?? "No customer";

        [ShowInInspector, ReadOnly]
        public SaleResult LastSaleResult { get; private set; }

        /// <summary>
        /// Fired when a new customer steps up. Passes the customer index and the customer.
        /// </summary>
        public event Action<int, Customer> OnCustomerReady;

        /// <summary>
        /// Fired when a customer is served. Passes the SaleResult.
        /// </summary>
        public event Action<SaleResult> OnCustomerServed;

        /// <summary>
        /// Fired when a customer is turned away.
        /// </summary>
        public event Action<Customer> OnCustomerTurnedAway;

        /// <summary>
        /// Fired when all customers have been served or turned away for the day.
        /// </summary>
        public event Action OnSellingDayComplete;

        private void Start()
        {
            GameManager.Instance.OnDayAdvanced += GenerateCustomers;
        }

        [Button("Generate Customers (Debug)")]
        void GenerateCustomers(DateTime date)
        {
            Customers = template.GenerateCustomers();
            CurrentCustomerIndex = -1;
            LastSaleResult = null;

            Debug.Log($"Generated {Customers.Count} customers for {date:MM/dd/yyyy}.");

            // automatically start the selling loop
            StartSellingDay();
        }

        /// <summary>
        /// Begins serving customers for the day. Moves to the first customer.
        /// </summary>
        [Button("Start Selling Day")]
        public void StartSellingDay()
        {
            if (Customers == null || Customers.Count == 0)
            {
                Debug.LogWarning("No customers to serve.");
                return;
            }

            CurrentCustomerIndex = 0;
            LastSaleResult = null;
            AnnounceCurrentCustomer();
        }

        /// <summary>
        /// Serve the current customer a product from the player's inventory.
        /// The product slot index corresponds to GameManager.Instance.ProductInventory.
        /// </summary>
        [Button("Serve Customer")]
        public SaleResult ServeCustomer(int productSlotIndex)
        {
            if (!IsSellingDay)
            {
                Debug.LogWarning("No selling day in progress.");
                return null;
            }

            var inventory = GameManager.Instance.ProductInventory;
            if (productSlotIndex < 0 || productSlotIndex >= inventory.Count)
            {
                Debug.LogError($"Invalid product slot index: {productSlotIndex}. Inventory has {inventory.Count} slots.");
                return null;
            }

            ProductSlot slot = inventory[productSlotIndex];
            if (slot.flower == null || slot.amount <= 0)
            {
                Debug.LogError("Selected product slot is empty.");
                return null;
            }

            Customer customer = CurrentCustomer;
            SaleResult result = customer.EvaluateProduct(slot.flower);
            LastSaleResult = result;

            if (result.accepted)
            {
                // pay the player
                GameManager.Instance.Money += result.payment;
                Debug.Log($"Customer bought {slot.flower.Name} for ${result.payment}!");

                // consume one unit from inventory
                slot.DecreaseAmount(1);
                inventory[productSlotIndex] = slot; // reassign because ProductSlot is a struct

                // remove slot if empty
                if (slot.amount <= 0)
                {
                    inventory.RemoveAt(productSlotIndex);
                }

                customer.state = Customer.CustomerState.Served;
            }
            else
            {
                Debug.Log($"Customer REJECTED {slot.flower.Name}: {result.rejectReason}");
                // product not accepted, customer stays to be served again or turned away
                // don't advance to next customer — player can try a different product or turn them away
                return result;
            }

            Debug.Log(result.ToString());
            OnCustomerServed?.Invoke(result);
            AdvanceToNextCustomer();

            return result;
        }

        /// <summary>
        /// Turn away the current customer without serving them.
        /// </summary>
        [Button("Turn Away Customer")]
        public void TurnAwayCustomer()
        {
            if (!IsSellingDay)
            {
                Debug.LogWarning("No selling day in progress.");
                return;
            }

            Customer customer = CurrentCustomer;
            customer.state = Customer.CustomerState.TurnedAway;
            Debug.Log($"Customer #{CurrentCustomerIndex + 1} turned away.");

            OnCustomerTurnedAway?.Invoke(customer);
            AdvanceToNextCustomer();
        }

        void AdvanceToNextCustomer()
        {
            CurrentCustomerIndex++;
            LastSaleResult = null;

            if (CurrentCustomerIndex >= Customers.Count)
            {
                // all customers done
                Debug.Log("All customers have been served. Selling day complete!");
                OnSellingDayComplete?.Invoke();
                return;
            }

            AnnounceCurrentCustomer();
        }

        void AnnounceCurrentCustomer()
        {
            Customer customer = CurrentCustomer;
            if (customer == null) return;

            InventorySummary summary = OrderManager.Instance.SummarizeInventory();
            if (summary == null)
            {
                Debug.LogError($"Inventory summary is null. Cannot generate order for customer #{CurrentCustomerIndex + 1}.");
                return;
            }

            customer.GenerateOrder(summary);

            Debug.Log($"--- Customer #{CurrentCustomerIndex + 1}/{Customers.Count} ---\n{customer.OrderSummary()}");
            OnCustomerReady?.Invoke(CurrentCustomerIndex, customer);
        }
    }
}