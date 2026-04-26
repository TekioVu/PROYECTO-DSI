using UnityEngine;

public class PlayerEntry : MonoBehaviour
{
    public string nombre;
    public float tiempo;
    public float progreso;
    public int muertes;

    public float Score()
    {
        return progreso * 2 - tiempo * 0.5f - muertes * 1.5f;
    }
}
