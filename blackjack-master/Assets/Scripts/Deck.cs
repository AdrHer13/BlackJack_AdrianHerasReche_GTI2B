using System;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;

    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */
        for(int i = 0; i<52; i++)
        {
            if (i == 0 || i == 13 || i == 26 || i == 39) values[i] = 11;
            if (i == 1 || i == 14 || i == 27 || i == 40) values[i] = 2;
            if (i == 2 || i == 15 || i == 28 || i == 41) values[i] = 3;
            if (i == 3 || i == 16 || i == 29 || i == 42) values[i] = 4;
            if (i == 4 || i == 17 || i == 30 || i == 43) values[i] = 5;
            if (i == 5 || i == 18 || i == 31 || i == 44) values[i] = 6;
            if (i == 6 || i == 19 || i == 32 || i == 45) values[i] = 7;
            if (i == 7 || i == 20 || i == 33 || i == 46) values[i] = 8;
            if (i == 8 || i == 21 || i == 34 || i == 47) values[i] = 9;
            if (i == 9 || i == 22 || i == 35 || i == 48) values[i] = 10;
            if (i == 10 || i == 23 || i == 36 || i == 49) values[i] = 10;
            if (i == 11 || i == 24 || i == 37 || i == 50) values[i] = 10;
            if (i == 12 || i == 25 || i == 38 || i == 51) values[i] = 10;
        }
    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */       
    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
        }
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        //Repartimos carta al jugador
        PushPlayer();
        
        if(player.GetComponent<CardHand>().points > 21)
        {
            //Jugador pierde
            finalMessage.text = "Has perdido!!";
        }

    }

    public void Stand()
    {
        dealer.GetComponent<CardHand>().InitialToggle();

        while (dealer.GetComponent<CardHand>().points <= 16)
        //Si el dealer tiene 17 o más deja de pedir cartas
        {
            PushDealer();
        }

        if(dealer.GetComponent<CardHand>().points > player.GetComponent<CardHand>().points && dealer.GetComponent<CardHand>().points <= 21 && player.GetComponent<CardHand>().points <= 21)
        {
            finalMessage.text = "El dealer te ha reventado papulince";
        }
        else if (dealer.GetComponent<CardHand>().points < player.GetComponent<CardHand>().points && player.GetComponent<CardHand>().points <= 21 && dealer.GetComponent<CardHand>().points <= 21)
        {
            finalMessage.text = "El player ha ganado a la banca, " + Environment.NewLine + "pero la banca siempre gana";
        } else if (dealer.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "El dealer ha explotido";
        }
            
         
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    
}
