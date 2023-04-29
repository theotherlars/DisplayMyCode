// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "TriggerConditions.h"
#include "CoreMinimal.h"

UENUM(BlueprintType)
enum class ETriggerCondition : uint8
{
	GeneratorState  UMETA(DisplayName = "Generator State"),
	DoorLockedState UMETA(DisplayName = "Door Locked State"),
	BoxTriggered    UMETA(DisplayName = "Box Is Triggered"),
	OtherTriggered	UMETA(DisplayName = "Other trigger has been triggered")
};

