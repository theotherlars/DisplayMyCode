// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "TriggerActions.h"
#include "CoreMinimal.h"

UENUM(BlueprintType)
enum class ETriggerAction : uint8
{
	NewLightState		UMETA(DisplayName = "New Light State"),
	LightFlicker		UMETA(DisplayName = "Light Flicker"),
	NewDoorLockedState	UMETA(DisplayName = "New Door Locked State"),
	MoveMonster			UMETA(DisplayName = "Move Monster"),
	Fracture			UMETA(DisplayName = "Fracture"),
	PlaySoundLocation	UMETA(DisplayName = "Play Sound At Location"),
	SpawnSmokeAtLocation UMETA(DisplayName = "Spawn Smok At Location")
};

