using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using Systems.Omp;
using UI.Inventory;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace UI.Inventory.Additional_UI
{
    public class FinalReport : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown suspects;
        [SerializeField] private TMP_Dropdown reasonsOfDeath;
        [SerializeField] private TMP_Dropdown motives;
        [SerializeField] private Button signTheReportButton;
        [SerializeField] private TMP_Text feedbackText;
        [SerializeField] private TMP_Text penaltySummaryText;
        [SerializeField] private TMP_Text penaltyDetailsText;
        [SerializeField] private Image suspectsBackground;
        [SerializeField] private Image reasonsOfDeathBackground;
        [SerializeField] private Image motivesBackground;
        [SerializeField] private Color correctColor = new Color(0f, 1f, 0f, 0.25f);
        [SerializeField] private Color incorrectColor = new Color(1f, 0f, 0f, 0.25f);

        private bool _isSubmitted;
        private InventoryUIItem _currentUiItem;

        private void OnEnable()
        {
            if (signTheReportButton)
            {
                signTheReportButton.onClick.AddListener(OnSignReportClicked);
            }
        }

        private void OnDisable()
        {
            if (signTheReportButton)
            {
                signTheReportButton.onClick.RemoveListener(OnSignReportClicked);
            }
        }
        
        public void PopulateFromUIItem(InventoryUIItem uiItem)
        {
            if (!uiItem)
                return;

            _currentUiItem = uiItem;
            
            if (feedbackText)
            {
                feedbackText.text = string.Empty;
            }

            SetDropdownOptions(suspects, uiItem.selectionOfSuspects);
            SetDropdownOptions(reasonsOfDeath, uiItem.selectionOfDeaths);
            SetDropdownOptions(motives, uiItem.selectionOfMotives);

            RefreshPenaltySection(false);
        }

        private void SetDropdownOptions(TMP_Dropdown dropdown, List<ResponseOptions> options)
        {
            if (!dropdown || options == null)
                return;

            dropdown.ClearOptions();

            var optionDataList = new List<TMP_Dropdown.OptionData>();
            for (int i = 0; i < options.Count; i++)
            {
                var responseOption = options[i];
                optionDataList.Add(new TMP_Dropdown.OptionData(responseOption != null ? responseOption.ResponseName : string.Empty));
            }

            dropdown.AddOptions(optionDataList);
            dropdown.value = 0;
            dropdown.RefreshShownValue();
        }

        private void OnSignReportClicked()
        {
            if (_isSubmitted || !_currentUiItem)
                return;

            RefreshPenaltySection(true);

            bool suspectCorrect = IsSelectionCorrect(suspects, _currentUiItem.selectionOfSuspects);
            bool deathCorrect = IsSelectionCorrect(reasonsOfDeath, _currentUiItem.selectionOfDeaths);
            bool motiveCorrect = IsSelectionCorrect(motives, _currentUiItem.selectionOfMotives);

            bool allCorrect = suspectCorrect && deathCorrect && motiveCorrect;

            ApplyHighlight(suspects, suspectsBackground, suspectCorrect);
            ApplyHighlight(reasonsOfDeath, reasonsOfDeathBackground, deathCorrect);
            ApplyHighlight(motives, motivesBackground, motiveCorrect);

            if (feedbackText)
            {
                if (allCorrect)
                {
                    feedbackText.text = "Все ответы верны. Отчёт отправлен.";
                }
                else
                {
                    string msg = "Ошибки: ";
                    List<string> wrong = new List<string>();
                    if (!suspectCorrect) wrong.Add("подозреваемый");
                    if (!deathCorrect) wrong.Add("причина смерти");
                    if (!motiveCorrect) wrong.Add("мотив");
                    feedbackText.text = msg + string.Join(", ", wrong) + ".";
                }
            }

            _isSubmitted = true;
            SetInteractable(false);
        }

        private void RefreshPenaltySection(bool finalizeBeforeRefresh)
        {
            OmpActionAnalyzer analyzer = OmpActionAnalyzer.Instance;
            if (!analyzer)
            {
                if (penaltySummaryText) penaltySummaryText.text = "Система анализа не активирована.";
                if (penaltyDetailsText) penaltyDetailsText.text = string.Empty;
                return;
            }

            if (finalizeBeforeRefresh)
            {
                analyzer.FinalizeAnalysis();
            }

            OmpActionAnalysisResult result = analyzer.BuildResult();

            if (penaltySummaryText)
            {
                penaltySummaryText.text = $"Штрафные баллы: {result.TotalPenalty:0}";
            }

            if (penaltyDetailsText)
            {
                if (result.Penalties.Count == 0)
                {
                    penaltyDetailsText.text = "Нарушений не зафиксировано.";
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (OmpPenaltyEntry penalty in result.Penalties)
                    {
                        builder.Append("• ");
                        builder.Append(penalty.Reason);
                        builder.Append(" (-");
                        builder.Append(penalty.Points.ToString("0.#"));
                        builder.AppendLine(")");
                    }

                    penaltyDetailsText.text = builder.ToString();
                }
            }
        }

        private bool IsSelectionCorrect(TMP_Dropdown dropdown, List<ResponseOptions> options)
        {
            if (!dropdown || options == null || options.Count == 0)
                return false;

            int index = dropdown.value;
            if (index < 0 || index >= options.Count)
                return false;

            ResponseOptions selected = options[index];
            return selected != null && selected.IsCorrect;
        }

        private void SetInteractable(bool interactable)
        {
            if (suspects) suspects.interactable = interactable;
            if (reasonsOfDeath) reasonsOfDeath.interactable = interactable;
            if (motives) motives.interactable = interactable;
            if (signTheReportButton) signTheReportButton.interactable = interactable;
        }

        private void ApplyHighlight(TMP_Dropdown dropdown, Image background, bool isCorrect)
        {
            if (background)
            {
                if (isCorrect) background.color = correctColor;
                else background.color = incorrectColor;
            }
            if (dropdown && dropdown.captionText)
            {
                if (isCorrect) dropdown.captionText.color = new Color(0f, 0.5f, 0f);
                else dropdown.captionText.color = new Color(0.6f, 0f, 0f);
            }
        }
    }
}
