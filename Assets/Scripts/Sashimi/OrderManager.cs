using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    [System.Serializable]
    public class Order
    {
        public string fishName;
        public int count;
    }

    public Text orderText; // 메모지 UI
    public List<string> fishTypes;

    private List<Order> currentOrders = new List<Order>();

    void Start()
    {
        GenerateRandomOrder();
    }

    void GenerateRandomOrder()
    {
        currentOrders.Clear();
        orderText.text = "";

        HashSet<string> chosenFish = new HashSet<string>();
        int orderCount = Random.Range(1, 4);

        while (currentOrders.Count < orderCount)
        {
            string randomFish = fishTypes[Random.Range(0, fishTypes.Count)];
            if (!chosenFish.Contains(randomFish))
            {
                int randomAmount = Random.Range(1, 4);
                currentOrders.Add(new Order { fishName = randomFish, count = randomAmount });
                chosenFish.Add(randomFish);
            }
        }

        UpdateOrderText();

    }

    void UpdateOrderText()
    {
        orderText.text = "";

        foreach (var order in currentOrders)
        {
            orderText.text += $"{order.fishName} x {order.count}\n";
        }
    }

    public void CompleteSashimi(string sashimiName)
    {
        foreach (var order in currentOrders)
        {
            if (order.fishName == sashimiName && order.count > 0)
            {
                order.count--;

                GameManager_s gm = FindObjectOfType<GameManager_s>();
                if (gm != null)
                {
                    gm.AddScore(100);
                }

                Debug.Log($"{sashimiName} 주문 1개 처리됨!");
                break;
            }
        }

        UpdateOrderText();

        // 모든 주문이 완료되었는지 확인
        if (IsAllOrdersComplete())
        {
            Debug.Log("모든 주문 완료! 새로운 주문 생성!");
            GenerateRandomOrder();
        }
    }

    private bool IsAllOrdersComplete()
    {
        foreach (var order in currentOrders)
        {
            if (order.count > 0) return false;
        }
        return true;
    }
}
