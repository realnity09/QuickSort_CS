using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSort : MonoBehaviour
{
    [SerializeField, Header("Range")]
    private int m_targetArrSize = 20;
    [SerializeField]
    private int m_min;
    [SerializeField]
    private int m_max;

    [SerializeField, Header("With UI")]
    private GameObject m_uiBar;
    [SerializeField]
    private Transform m_instantiateParent;

    private List<ElementBar> m_barList = new List<ElementBar>();
    private int m_pivot;

    [SerializeField, Header("Delay")]
    private float m_delay = 0.0125f;
    private WaitForSeconds m_wfs;

    #region Button event
    public void CreateArr()
    {
        RemoveAllElements();

        for(int i = 0; i < m_targetArrSize; i++)
        {
            m_barList.Add(Instantiate(m_uiBar, m_instantiateParent).GetComponent<ElementBar>().Init(Random.Range(m_min, m_max)));
        }
    }

    private void RemoveAllElements()
    {
        for(int i = 0; i < m_barList.Count; i++)
        {
            Destroy(m_barList[i].gameObject);
        }
        m_barList.Clear();
    }

    public void StartQuickSort()
    {
        if (m_barList.Count == 0)
        {
            return;
        }
        m_wfs = new WaitForSeconds(m_delay);
        StartCoroutine(ShowProcessing(m_barList, 0, m_barList.Count - 1));
    }
    #endregion

    #region Quick sort funcs
    private void StartQuickSortStraight()
    {
        var arr = CreateRandomArr();
        QuickSortRecursive(arr, 0, arr.Length - 1);
    }

    private void QuickSortRecursive(int[] arr, int left, int right)
    {
        if(left >= right)
        {
            return;
        }

        int pivot = Sort(arr, left, right);

        //Left partition
        Sort(arr, left, pivot - 1);
        //Right partition
        Sort(arr, pivot + 1, right);
    }

    private int Sort(int[] arr, int low, int pivot)
    {
        int high = pivot - 1;

        //low와 high가 교차되면 반복문 종료
        while (low <= high)
        {
            //Ascending(오름차순)
            for (; low <= high && arr[low] <= arr[pivot]; low++) { }
            for (; high >= low && arr[high] >= arr[pivot]; high--) { }

            //low가 high와 교차되어 있지 않으면 Swap
            if (low <= high)
            {
                Swap(arr, low, high);
            }
        }

        //low와 pivot 값을 Swap
        Swap(arr, low, pivot);

        //low 값이 pivot이 되므로 해당 값을 반환
        return low;
    }

    private int[] CreateRandomArr()
    {
        var arr = new int[m_targetArrSize];
        for (int i = 0; i < m_targetArrSize; i++)
        {
            arr[i] = Random.Range(m_min, m_max);
        }

        return arr;
    }

    private void Swap(int[] arr, int a, int b)
    {
        int tmp = arr[a];
        arr[a] = arr[b];
        arr[b] = tmp;
    }
    #endregion

    #region Quick sort coroutines
    IEnumerator ShowProcessing(List<ElementBar> arr, int start, int pivot)
    {
        yield return StartCoroutine(QuickSortCoroutine(arr, start, pivot));

        for(int i = 0; i < arr.Count; i++)
        {
            arr[i].SetCompleteColor();
            yield return m_wfs;
        }
    }

    private IEnumerator QuickSortCoroutine(List<ElementBar> arr, int left, int right)
    {
        //배열이 더이상 쪼개지지 않으면 종료
        if(left >= right)
        {
            yield break;
        }

        yield return StartCoroutine(SortCoroutine(arr, left, right));

        yield return m_wfs;

        //Left partition
        yield return StartCoroutine(QuickSortCoroutine(arr, left, m_pivot - 1));
        //Right partition
        yield return StartCoroutine(QuickSortCoroutine(arr, m_pivot + 1, right));
    }

    private IEnumerator SortCoroutine(List<ElementBar> arr, int low, int pivot)
    {
        int high = pivot - 1;
        arr[low].SetLowColor();
        arr[high].SetHighColor();
        arr[pivot].SetPivotColor();

        //low와 high가 교차되면 반복문 종료
        while (low <= high)
        {
            while(arr[low].Item <= arr[pivot].Item)
            {
                yield return m_wfs;
                arr[low].SetDefaultColor();
                low++;
                if (low <= high)
                {
                    arr[low].SetLowColor();
                }
                else
                {
                    break;
                }
            }
            while(arr[high].Item >= arr[pivot].Item)
            {
                yield return m_wfs;
                arr[high].SetDefaultColor();
                high--;
                if (high >= low)
                {
                    arr[high].SetHighColor();
                }
                else
                {
                    break;
                }
            }

            //low가 high와 교차되어 있지 않으면 Swap
            if (low <= high)
            {
                Swap(arr, low, high);
            }
        }

        //low와 pivot 값을 Swap
        Swap(arr, low, pivot);
        arr[pivot].SetDefaultColor();

        //low 값이 pivot이 되므로 해당 값을 반환
        m_pivot = low;
    }

    private void Swap(List<ElementBar> arr, int a, int b)
    {
        int tmp = arr[a].Item;
        arr[a].Item = arr[b].Item;
        arr[b].Item = tmp;
    }
    #endregion
}
