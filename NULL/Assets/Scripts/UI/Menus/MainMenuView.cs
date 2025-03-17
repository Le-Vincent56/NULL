using DG.Tweening;
using DG.Tweening.Core.Easing;
using NULL.Extensions.VisualElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace NULL.UI.Menus
{
    public class MainMenuView : MenuView
    {
        [Header("Fields")]
        [SerializeField] string title = "null";
        [SerializeField] float marginDuration = 0.5f;
        [SerializeField] float endScrollDuration = 2f; // total duration of the scroll tween
        [SerializeField] float middleScrollDuration = 0.25f;
        [SerializeField] float settleDelay = 0.5f;
        [SerializeField] float titlePause = 3f; // pause after the margins return
        [SerializeField] float targetMargin = 20f;   // example margin value
        [SerializeField] float scrollDistance = 30f; // vertical scroll distance
        [SerializeField] int scrollCycles = 5;

        [SerializeField] AnimationCurve rampCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        // We'll now use a container and a list of Labels (one per character) instead of one title Label.
        private VisualElement titleContainer;
        private readonly List<Label> letterLabels = new List<Label>();

        private const string scrambleCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()|";

        public override IEnumerator InitializeView(int size = 3)
        {
            // Initialize the Choices array with the given size
            choices = new Choice[size];

            // Assign the root Visual Element and clear it to ensure re-initialization
            root = document.rootVisualElement;
            root.Clear();

            // Set the style sheet
            root.styleSheets.Add(styleSheet);

            // Create the root container
            container = root.CreateChild("container");

            // Create the main menu
            VisualElement mainMenu = container.CreateChild("main-menu");

            // Create a container for the title (instead of a single label)
            titleContainer = mainMenu.CreateChild("title-header");
            CreateLetterElements();

            // Create the container for the choices
            VisualElement choiceContainer = mainMenu.CreateChild("choices-container");

            // Iterate a number of times equal to the size of the choices array
            for (int i = 0; i < size; i++)
            {
                // Create the choice and store it
                Choice choice = choiceContainer.CreateChild<Choice>("choice");

                // Assign a class based on order
                if (i % 2 == 0)
                    choice.AddToClassList("even");
                else
                    choice.AddToClassList("odd");

                choices[i] = choice;
            }

            // Set text
            choices[0].Set("start");
            choices[1].Set("settings");
            choices[2].Set("quit");

            yield return null;

            AnimateLetters();
        }

        /// <summary>
        /// Splits the final title string into individual letter labels.
        /// </summary>
        private void CreateLetterElements()
        {
            // Clear the lists
            titleContainer.Clear();
            letterLabels.Clear();

            // Iterate through each letter
            for (int i = 0; i < title.Length; i++)
            {
                // Create a label for each letter
                Label letterLabel = new Label(title[i].ToString());
                letterLabel.AddClass("letter");
                letterLabel.style.marginRight = 0;
                letterLabel.style.top = 0;
                letterLabel.style.opacity = 1;

                // Add the letter to the title container and the list of labels
                titleContainer.Add(letterLabel);
                letterLabels.Add(letterLabel);
            }
        }

        private void AnimateLetters()
        {
            for (int i = 0; i < letterLabels.Count; i++)
            {
                Label letter = letterLabels[i];
                string originalChar = letter.text;
                // Even letters scroll one way, odd letters the opposite.
                float direction = (i % 2 == 0) ? -1 : 1;

                Sequence masterSequence = DOTween.Sequence();

                // (1) Margin tween to separate the letters.
                Tween marginAdjustmentTween = DOTween.To(
                    () => letter.style.marginRight.value.value,
                    x => letter.style.marginRight = x,
                    targetMargin,
                    marginDuration
                ).SetEase(Ease.OutQuad);
                masterSequence.Append(marginAdjustmentTween);

                // (2) Settle delay.
                masterSequence.AppendInterval(settleDelay);

                // (3) Start Scroll Cycle – using one continuous motion with a transitional tween.
                Sequence startScrollCycle = DOTween.Sequence();

                // Part 1: Positive tween from 0 to positive offset.
                Tween startPosScrollTween = DOTween.To(
                    () => letter.style.top.value.value,
                    x => letter.style.top = x,
                    scrollDistance * direction,
                    endScrollDuration / 2f
                ).SetEase(rampCurve);
                Tween startPosOpacityTween = DOTween.To(
                    () => letter.style.opacity.value,
                    x => letter.style.opacity = x,
                    0f,
                    endScrollDuration / 2f
                ).SetEase(rampCurve);
                startScrollCycle.Append(startPosScrollTween);
                startScrollCycle.Join(startPosOpacityTween);

                // Part 2: Transition tween – smoothly interpolate from positive to negative offset.
                // Use a short duration (for example, 1/10th of endScrollDuration).
                Tween transitionTween = DOTween.To(
                    () => letter.style.top.value.value,
                    x => letter.style.top = x,
                    scrollDistance * -direction,
                    endScrollDuration / 10f
                ).SetEase(rampCurve)
                .OnUpdate(() =>
                {
                    // When halfway through the transition, update the letter text.
                    // We check if the current top value has crossed the midpoint between the two offsets.
                    float midPoint = (scrollDistance * direction + scrollDistance * -direction) / 2f;
                    if ((direction > 0 && letter.style.top.value.value < midPoint) ||
                        (direction < 0 && letter.style.top.value.value > midPoint))
                    {
                        // Update text only once.
                        if (letter.text == originalChar)
                        {
                            letter.text = scrambleCharacters[Random.Range(0, scrambleCharacters.Length)].ToString();
                        }
                    }
                });
                startScrollCycle.Append(transitionTween);

                // Part 3: Negative tween from negative offset back to 0.
                Tween negativeTween = DOTween.To(
                    () => letter.style.top.value.value,
                    x => letter.style.top = x,
                    0f,
                    endScrollDuration / 2f
                ).SetEase(rampCurve);
                Tween negativeOpacityTween = DOTween.To(
                    () => letter.style.opacity.value,
                    x => letter.style.opacity = x,
                    1f,
                    endScrollDuration / 2f
                ).SetEase(rampCurve);
                startScrollCycle.Append(negativeTween);
                startScrollCycle.Join(negativeOpacityTween);

                masterSequence.Append(startScrollCycle);

                // (4) Middle Scroll Cycle – loops with constant speed (linear easing).
                Sequence singleScrollCycle = DOTween.Sequence();
                // Positive tween (linear).
                Tween positiveScrollTween = DOTween.To(
                    () => letter.style.top.value.value,
                    x => letter.style.top = x,
                    scrollDistance * direction,
                    middleScrollDuration
                ).SetEase(Ease.Linear);
                Tween positiveOpacityTween = DOTween.To(
                    () => letter.style.opacity.value,
                    x => letter.style.opacity = x,
                    0f,
                    middleScrollDuration
                ).SetEase(Ease.Linear);
                singleScrollCycle.Append(positiveScrollTween);
                singleScrollCycle.Join(positiveOpacityTween);

                // Callback to randomize letter and set starting position for negative tween.
                singleScrollCycle.AppendCallback(() =>
                {
                    letter.text = scrambleCharacters[Random.Range(0, scrambleCharacters.Length)].ToString();
                    letter.style.top = scrollDistance * -direction;
                });

                // Negative tween (linear).
                Tween negativeScrollTween = DOTween.To(
                    () => letter.style.top.value.value,
                    x => letter.style.top = x,
                    0f,
                    middleScrollDuration
                ).SetEase(Ease.Linear);
                Tween negativeScrollOpacityTween = DOTween.To(
                    () => letter.style.opacity.value,
                    x => letter.style.opacity = x,
                    1f,
                    middleScrollDuration
                ).SetEase(Ease.Linear);
                singleScrollCycle.Append(negativeScrollTween);
                singleScrollCycle.Join(negativeScrollOpacityTween);

                // Loop the middle scroll cycle for (scrollCycles - 2) times.
                Sequence scrollSequence = DOTween.Sequence();
                scrollSequence.Append(singleScrollCycle).SetLoops(scrollCycles - 2);
                masterSequence.Append(scrollSequence);

                // (5) Final Scroll Cycle – use a continuous tween similar to the start cycle.
                Sequence finalScrollCycle = DOTween.Sequence();
                // Part 1: Positive tween from 0 to positive offset.
                Tween finalPositiveTween = DOTween.To(
                    () => letter.style.top.value.value,
                    x => letter.style.top = x,
                    scrollDistance * direction,
                    endScrollDuration / 2f
                ).SetEase(rampCurve);
                Tween finalPositiveOpacityTween = DOTween.To(
                    () => letter.style.opacity.value,
                    x => letter.style.opacity = x,
                    0f,
                    endScrollDuration / 2f
                ).SetEase(rampCurve);
                finalScrollCycle.Append(finalPositiveTween);
                finalScrollCycle.Join(finalPositiveOpacityTween);

                // Part 2: Transition tween from positive to negative offset.
                Tween finalTransitionTween = DOTween.To(
                    () => letter.style.top.value.value,
                    x => letter.style.top = x,
                    scrollDistance * -direction,
                    endScrollDuration / 10f
                ).SetEase(rampCurve)
                .OnUpdate(() =>
                {
                    // When passing the midpoint, reset letter text to the original.
                    float midPoint = (scrollDistance * direction + scrollDistance * -direction) / 2f;
                    if ((direction > 0 && letter.style.top.value.value < midPoint) ||
                        (direction < 0 && letter.style.top.value.value > midPoint))
                    {
                        if (letter.text != originalChar)
                        {
                            letter.text = originalChar;
                        }
                    }
                });
                finalScrollCycle.Append(finalTransitionTween);

                // Part 3: Negative tween from negative offset back to 0.
                Tween finalNegativeTween = DOTween.To(
                    () => letter.style.top.value.value,
                    x => letter.style.top = x,
                    0f,
                    endScrollDuration / 2f
                ).SetEase(rampCurve);
                Tween finalNegativeOpacityTween = DOTween.To(
                    () => letter.style.opacity.value,
                    x => letter.style.opacity = x,
                    1f,
                    endScrollDuration / 2f
                ).SetEase(rampCurve);
                finalScrollCycle.Append(finalNegativeTween);
                finalScrollCycle.Join(finalNegativeOpacityTween);

                masterSequence.Append(finalScrollCycle);

                // (6) Final settle delay.
                masterSequence.AppendInterval(settleDelay);

                // (7) Margin correction tween.
                Tween marginCorrectionTween = DOTween.To(
                    () => letter.style.marginRight.value.value,
                    x => letter.style.marginRight = x,
                    0f,
                    marginDuration
                ).SetEase(Ease.OutQuad);
                masterSequence.Append(marginCorrectionTween);

                // (8) Final delay before looping the entire sequence.
                masterSequence.AppendInterval(titlePause);

                // Only the master sequence loops infinitely.
                masterSequence.SetLoops(-1);
            }
        }
    }
}
