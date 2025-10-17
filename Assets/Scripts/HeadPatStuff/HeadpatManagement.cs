using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeadpatManagement : MonoBehaviour
{
	public int headpats;
	public Animator catAnimator, cameraAnimator;
	public TextMeshProUGUI signText;
	public bool isHeadPatting = false, isSecondCat = false, isCatAsleep = true;
	public InputManager inputManager;

	public CanvasGroup cg;
	private void Update()
	{
		if (isCatAsleep)
		{
			return;
		}
		if (isSecondCat)
		{
			if (Input.GetMouseButtonDown(1) && !isHeadPatting)
			{
				HeadPatCat();

			}

			if (Input.GetMouseButtonUp(1) && isHeadPatting)
			{
				UnHeadPatCat();

			}
		}
		else
		{
			if (Input.GetMouseButtonDown(0) && !isHeadPatting)
			{
				HeadPatCat();

			}

			if (Input.GetMouseButtonUp(0) && isHeadPatting)
			{
				UnHeadPatCat();

			}
		}

		if (inputManager.pause)
		{
			SleepenCat();
		}

	}
	public void HeadPatCat()
	{
		// increments count
		// headpats cat
		//print("Headpat");
		isHeadPatting = true;
		catAnimator.SetTrigger("HeadPat");
		headpats++;
		SetSignText();
		catAnimator.ResetTrigger("Idle");
	}

	public void UnHeadPatCat()
	{
		// returns to idle
		print("UnHeadpat");
		isHeadPatting = false;
		catAnimator.SetTrigger("Idle");


	}

	public void SetSignText()
	{
		signText.text = $"{headpats}";
	}

	public void AwakenCat()
	{
		isCatAsleep = false;
		catAnimator.SetBool("Asleep", isCatAsleep);
		cameraAnimator.SetBool("Asleep", isCatAsleep);
		cg.alpha = 0;
	}

	public void SleepenCat()
	{
		isCatAsleep = true;
		UnHeadPatCat();
		catAnimator.SetBool("Asleep", isCatAsleep);
		cameraAnimator.SetBool("Asleep", isCatAsleep);
		cg.alpha = 1;
	}


	void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus && isHeadPatting)
		{
			// If the user alt-tabs while patting, reset state
			UnHeadPatCat();
		}
	}
}
