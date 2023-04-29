// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "PowerGeneratorBase.generated.h"

UCLASS()
class GERMINATE_API APowerGeneratorBase : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	APowerGeneratorBase();

	// Turns on the Power Generator if there is any rods inserted
	UFUNCTION(BlueprintCallable,BlueprintImplementableEvent, meta=(ForceAsFunction), Category="Power Generator")
	void TurnOn();

	// Turns of the Power Generator
	UFUNCTION(BlueprintCallable,BlueprintImplementableEvent, meta=(ForceAsFunction), Category="Power Generator")
	void TurnOff();
	
	// Inserts a power rod
	UFUNCTION(BlueprintCallable,BlueprintImplementableEvent, meta=(ForceAsFunction), Category="Power Generator")
	void InsertPowerRod();

	// Removes a power rod
	UFUNCTION(BlueprintCallable,BlueprintImplementableEvent, meta=(ForceAsFunction), Category="Power Generator")
	void RemovePowerRod();
	
	// Will drain power over time at a specified interval
	UFUNCTION(BlueprintCallable,BlueprintImplementableEvent, meta=(ForceAsFunction), Category="Power Generator")
	void DrainPower();

	UFUNCTION(BlueprintCallable,BlueprintImplementableEvent, meta=(ForceAsFunction), Category="Power Generator")
	int32 GetInsertedPowerRodAmount();
	
	// Returns if the Power Generator is on or off
	UFUNCTION(BlueprintCallable,BlueprintImplementableEvent,BlueprintPure, meta=(ForceAsFunction), Category="Power Generator")
	bool GetActive();

	UFUNCTION(BlueprintCallable,BlueprintImplementableEvent, meta=(ForceAsFunction), Category="Power Generator")
	void SetActive();

	UFUNCTION(BlueprintCallable,BlueprintImplementableEvent,BlueprintPure, meta=(ForceAsFunction), Category="Power Generator")
	float GetPowerLevel();

	UFUNCTION(BlueprintCallable,BlueprintImplementableEvent, meta=(ForceAsFunction), Category="Power Generator")
	void SetPowerLevel();
	
protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;
		
public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;
};
