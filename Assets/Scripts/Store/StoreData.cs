using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Store
{
    public string storeName;
    public string address;
    public int hungry;
    public string menu;
    public int foodPrice;
}

[System.Serializable]
public class StoreList
{
    public List<Store> stores;
}
