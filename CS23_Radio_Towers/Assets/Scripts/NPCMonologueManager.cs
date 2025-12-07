using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCMonologueManager : MonoBehaviour {

       public GameObject monologueBox;
       public TMP_Text monologueText;
       public float typingSpeed = 0.05f; // Speed of typing in seconds
       public string[] monologue;
       public int counter = 0;
       public int monologueLength;
       
       private bool isTyping = false;
       private Coroutine typingCoroutine;

       void Start(){
              monologueBox.SetActive(false);
              monologueLength = monologue.Length; //allows us test dialogue without an NPC
       }

       void Update(){
              //temporary testing before NPC is created
              if (Input.GetKeyDown("o")){
                     monologueBox.SetActive(true);
              }
              if (Input.GetKeyDown("p")){
                     monologueBox.SetActive(false);
                     monologueText.text = "..."; //reset text
                     counter = 0; //reset counter
              }
       }

       public void OpenMonologue(){
              monologueBox.SetActive(true);
 
              //auto-loads the first line of monologue with typing effect
              StartTyping(monologue[0]);
              counter = 1;
       }

       public void CloseMonologue(){
              StopTyping();
              monologueBox.SetActive(false);
              monologueText.text = "..."; //reset text
              counter = 0; //reset counter
       }

       public void LoadMonologueArray(string[] NPCscript, int scriptLength){
              monologue = NPCscript;
              monologueLength = scriptLength;
       }

        //function for the button to display next line of dialogue
       public void MonologueNext(){
              // If currently typing, complete the current line instantly
              if (isTyping)
              {
                     CompleteTyping();
                     return;
              }

              if (counter < monologueLength){
                     StartTyping(monologue[counter]);
                     counter +=1;
              }
              else { //when lines are complete:
                     monologueBox.SetActive(false); //turn off the dialogue display
                     monologueText.text = "..."; //reset text
                     counter = 0; //reset counter
              }
       }

       // Start typing effect for a specific text
       private void StartTyping(string textToType)
       {
              StopTyping();
              monologueText.text = "";
              typingCoroutine = StartCoroutine(TypeText(textToType));
       }

       // Stop the current typing effect
       private void StopTyping()
       {
              if (typingCoroutine != null)
              {
                     StopCoroutine(typingCoroutine);
                     typingCoroutine = null;
              }
              isTyping = false;
       }

       // Complete the typing instantly
       private void CompleteTyping()
       {
              StopTyping();
              // The text will be completed in the coroutine's finally block
       }

       // Coroutine to simulate typing effect
       private IEnumerator TypeText(string textToType)
       {
              isTyping = true;
              
              for (int i = 0; i <= textToType.Length; i++)
              {
                     monologueText.text = textToType.Substring(0, i);
                     yield return new WaitForSeconds(typingSpeed);
              }
              
              isTyping = false;
       }

}