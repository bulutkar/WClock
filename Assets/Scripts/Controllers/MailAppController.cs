using System.Diagnostics;
using UnityEngine;

namespace Controllers
{
    public class MailAppController : MonoBehaviour
    {
        public void OpenDefaultMailApp()
        {
            Process.Start("outlookmail://");
        }
    }
}
