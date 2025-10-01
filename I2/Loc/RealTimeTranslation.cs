using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000469 RID: 1129
	public class RealTimeTranslation : MonoBehaviour
	{
		// Token: 0x0600331A RID: 13082 RVA: 0x001553F4 File Offset: 0x001535F4
		public void OnGUI()
		{
			GUILayout.Label("Translate:", Array.Empty<GUILayoutOption>());
			this.OriginalText = GUILayout.TextArea(this.OriginalText, new GUILayoutOption[]
			{
				GUILayout.Width((float)Screen.width)
			});
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("English -> Español", new GUILayoutOption[]
			{
				GUILayout.Height(100f)
			}))
			{
				this.StartTranslating("en", "es");
			}
			if (GUILayout.Button("Español -> English", new GUILayoutOption[]
			{
				GUILayout.Height(100f)
			}))
			{
				this.StartTranslating("es", "en");
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.TextArea("Multiple Translation with 1 call:\n'This is an example' -> en,zh\n'Hola' -> en", Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Multi Translate", new GUILayoutOption[]
			{
				GUILayout.ExpandHeight(true)
			}))
			{
				this.ExampleMultiTranslations_Async();
			}
			GUILayout.EndHorizontal();
			GUILayout.TextArea(this.TranslatedText, new GUILayoutOption[]
			{
				GUILayout.Width((float)Screen.width)
			});
			GUILayout.Space(10f);
			if (this.IsTranslating)
			{
				GUILayout.Label("Contacting Google....", Array.Empty<GUILayoutOption>());
			}
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x00155537 File Offset: 0x00153737
		public void StartTranslating(string fromCode, string toCode)
		{
			this.IsTranslating = true;
			GoogleTranslation.Translate(this.OriginalText, fromCode, toCode, new GoogleTranslation.fnOnTranslated(this.OnTranslationReady));
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x00155559 File Offset: 0x00153759
		private void OnTranslationReady(string Translation, string errorMsg)
		{
			this.IsTranslating = false;
			if (errorMsg != null)
			{
				Debug.LogError(errorMsg);
				return;
			}
			this.TranslatedText = Translation;
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x00155574 File Offset: 0x00153774
		public void ExampleMultiTranslations_Blocking()
		{
			Dictionary<string, TranslationQuery> dictionary = new Dictionary<string, TranslationQuery>();
			GoogleTranslation.AddQuery("This is an example", "en", "es", dictionary);
			GoogleTranslation.AddQuery("This is an example", "auto", "zh", dictionary);
			GoogleTranslation.AddQuery("Hola", "es", "en", dictionary);
			if (!GoogleTranslation.ForceTranslate(dictionary, true))
			{
				return;
			}
			Debug.Log(GoogleTranslation.GetQueryResult("This is an example", "en", dictionary));
			Debug.Log(GoogleTranslation.GetQueryResult("This is an example", "zh", dictionary));
			Debug.Log(GoogleTranslation.GetQueryResult("This is an example", "", dictionary));
			Debug.Log(dictionary["Hola"].Results[0]);
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x00155628 File Offset: 0x00153828
		public void ExampleMultiTranslations_Async()
		{
			this.IsTranslating = true;
			Dictionary<string, TranslationQuery> dictionary = new Dictionary<string, TranslationQuery>();
			GoogleTranslation.AddQuery("This is an example", "en", "es", dictionary);
			GoogleTranslation.AddQuery("This is an example", "auto", "zh", dictionary);
			GoogleTranslation.AddQuery("Hola", "es", "en", dictionary);
			GoogleTranslation.Translate(dictionary, new GoogleTranslation.fnOnTranslationReady(this.OnMultitranslationReady), true);
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x00155694 File Offset: 0x00153894
		private void OnMultitranslationReady(Dictionary<string, TranslationQuery> dict, string errorMsg)
		{
			if (!string.IsNullOrEmpty(errorMsg))
			{
				Debug.LogError(errorMsg);
				return;
			}
			this.IsTranslating = false;
			this.TranslatedText = "";
			this.TranslatedText = this.TranslatedText + GoogleTranslation.GetQueryResult("This is an example", "es", dict) + "\n";
			this.TranslatedText = this.TranslatedText + GoogleTranslation.GetQueryResult("This is an example", "zh", dict) + "\n";
			this.TranslatedText = this.TranslatedText + GoogleTranslation.GetQueryResult("This is an example", "", dict) + "\n";
			this.TranslatedText += dict["Hola"].Results[0];
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x00155757 File Offset: 0x00153957
		public bool IsWaitingForTranslation()
		{
			return this.IsTranslating;
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x0015575F File Offset: 0x0015395F
		public string GetTranslatedText()
		{
			return this.TranslatedText;
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x00155767 File Offset: 0x00153967
		public void SetOriginalText(string text)
		{
			this.OriginalText = text;
		}

		// Token: 0x040020CC RID: 8396
		private string OriginalText = "This is an example showing how to use the google translator to translate chat messages within the game.\nIt also supports multiline translations.";

		// Token: 0x040020CD RID: 8397
		private string TranslatedText = string.Empty;

		// Token: 0x040020CE RID: 8398
		private bool IsTranslating;
	}
}
