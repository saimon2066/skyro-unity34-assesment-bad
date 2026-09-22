using UnityEngine;
using UnityEngine.UI;

public class HudStuff : MonoBehaviour
{
    public Text txt;
    public Text txt2;
    public string last;

    public void upd(string s)
    {
        last = s;
        if (txt != null)
        {
            txt.text = s;
        }
    }

    public void setScore(int n)
    {
        if (txt2 != null)
        {
            txt2.text = "" + n;
        }
    }

    void Update()
    {
        // TODO hook this to gm
        if (txt == null)
        {
            var go = GameObject.Find("HPText");
            if (go != null) txt = go.GetComponent<Text>();
        }
    }
}
