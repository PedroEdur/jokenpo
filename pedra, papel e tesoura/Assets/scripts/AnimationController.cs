using UnityEngine;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    public Image player1Reveal;
    public Image player2Reveal;

    public Sprite pedra;
    public Sprite papel;
    public Sprite tesoura;

    public void MostrarEscolhas(GameManager.escolhadojogador p1,
                                GameManager.escolhadojogador p2)
    {
        player1Reveal.enabled = true;
        player2Reveal.enabled = true;

        player1Reveal.sprite = ObterSprite(p1);
        player2Reveal.sprite = ObterSprite(p2);
    }

    public void EsconderEscolhas()
    {
        player1Reveal.enabled = false;
        player2Reveal.enabled = false;
    }

    private Sprite ObterSprite(GameManager.escolhadojogador escolha)
    {
        switch (escolha)
        {
            case GameManager.escolhadojogador.Pedra:
                return pedra;

            case GameManager.escolhadojogador.Papel:
                return papel;

            case GameManager.escolhadojogador.Tesoura:
                return tesoura;

            default:
                return null;
        }
    }
}