using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerRegister : MonoBehaviour
{
    public static SkeletonManager SkeletonManager;


    public void Register() {
        SkeletonManager = GetComponent<SkeletonManager>();
        //string Id = Md5Sum(gameObject.name);
        Managers.managers.AddManager(gameObject.name, SkeletonManager);
    }

    public static string Md5Sum(string input) {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < hash.Length; i++) {
            sb.Append(hash[i].ToString("X2"));//大  "X2",小"x2"  
        }
        return sb.ToString();

    }
}
