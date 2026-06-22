using UnityEngine;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    [Header("Imagens dos jogadores")]
    public Image player1Image;
    public Image player2Image;

    [Header("Sprites")]
    public Sprite pedra;
    public Sprite papel;
    public Sprite tesoura;

    public void MostrarEscolhas(string escolhaP1, string escolhaP2)
    {
        player1Image.sprite = ObterSprite(escolhaP1);
        player2Image.sprite = ObterSprite(escolhaP2);

        Debug.Log("Escolhas reveladas!");
    }

    private Sprite ObterSprite(string escolha)
    {
        switch (escolha)
        {
            case "Pedra":
                return pedra;

            case "Papel":
                return papel;

            case "Tesoura":
                return tesoura;

            default:
                return null;
        }
    }

   

}