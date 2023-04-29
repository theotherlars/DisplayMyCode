// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "PowerGeneratorBase.h"
#include "Components/ActorComponent.h"
#include "TriggerConditions.h"
#include "TriggerActions.h"
#include "LightBase.h"
#include "DoorBase.h"
#include "TriggerComponent.generated.h"

USTRUCT(BlueprintType)
struct FTriggerConditionStruct
{
	GENERATED_BODY()

public:
	UPROPERTY(EditAnywhere,BlueprintReadWrite, Category="Trigger System")
	ETriggerCondition TriggerCondition;

	// using meta to hide variables that are not needed when a trigger condition is selected
	UPROPERTY(EditAnywhere,BlueprintReadWrite, meta=(EditCondition = "TriggerCondition == ETriggerCondition::GeneratorState",EditConditionHides), Category="Trigger System")
	bool bPowerGeneratorOn;

	UPROPERTY(EditAnywhere,BlueprintReadWrite, meta=(EditCondition = "TriggerCondition == ETriggerCondition::DoorLockedState",EditConditionHides), Category="Trigger System")
	TSoftObjectPtr<AActor> DoorRef;

	UPROPERTY(EditAnywhere,BlueprintReadWrite, meta=(EditCondition = "TriggerCondition == ETriggerCondition::DoorLockedState",EditConditionHides), Category="Trigger System")
	bool bDoorLocked;

	UPROPERTY(EditAnywhere,BlueprintReadWrite, meta=(EditCondition = "TriggerCondition == ETriggerCondition::BoxTriggered",EditConditionHides), Category="Trigger System")
	TSoftObjectPtr<AActor> BoxRef;

	UPROPERTY(EditAnywhere,BlueprintReadWrite, meta=(EditCondition = "TriggerCondition == ETriggerCondition::OtherTriggered", EditConditionHides), Category="Trigger System")
	TSoftObjectPtr<AActor> TriggerObjectReference;
	
	UPROPERTY(EditAnywhere,BlueprintReadWrite, meta=(EditCondition = "TriggerCondition == ETriggerCondition::OtherTriggered", EditConditionHides), Category="Trigger System")
	bool bRequiredTriggerState;
};

USTRUCT(BlueprintType)
struct FTriggerActionStruct
{
	GENERATED_BODY()

public:
	UPROPERTY(EditAnywhere,BlueprintReadWrite, Category="Trigger System")
	ETriggerAction TriggerAction;


	/*
	TODO: Use the specific class type instead of generic actor to lock down what type the designer can reference in scene.
	Since the class types are not childed from C++ class I can't reference the class in C++ 
	*/

	// using meta to hide variables that are not needed when a trigger action is selected
	UPROPERTY(EditAnywhere,BlueprintReadWrite, meta=(EditCondition = "TriggerAction == ETriggerAction::NewLightState || TriggerAction == ETriggerAction::LightFlicker",EditConditionHides), Category="Trigger System")
	TSoftObjectPtr<AActor> LightReference;

	UPROPERTY(EditAnywhere,BlueprintReadWrite, meta=(EditCondition = "TriggerAction == ETriggerAction::NewLightState || TriggerAction == ETriggerAction::LightFlicker",EditConditionHides), Category="Trigger System")
	TSoftObjectPtr<ALightBase> LightBaseReference; // TESTING OUT NEW LIGHT BASE CLASS WITHOUT BREAKING THE ORIGINAL ACTOR REFERENCE
	
	UPROPERTY(EditAnywhere,BlueprintReadWrite, meta=(EditCondition = "TriggerAction == ETriggerAction::NewLightState",EditConditionHides), Category="Trigger System")
	bool bNewLightState;

	UPROPERTY(EditAnywhere,BlueprintReadWrite, meta=(EditCondition = "TriggerAction == ETriggerAction::LightFlicker",EditConditionHides), Category="Trigger System")
	bool bNewFlickerState; 

	UPROPERTY(EditAnywhere,BlueprintReadWrite, meta=(EditCondition = "TriggerAction == ETriggerAction::NewDoorLockedState",EditConditionHides), Category="Trigger System")
	TSoftObjectPtr<AActor> DoorReference;

	UPROPERTY(EditAnywhere,BlueprintReadWrite, meta=(EditCondition = "TriggerAction == ETriggerAction::NewDoorLockedState",EditConditionHides), Category="Trigger System")
	TSoftObjectPtr<ADoorBase> DoorBaseReference; // TESTING OUT NEW DOOR BASE CLASS WITHOUT BREAKING THE ORIGINAL ACTOR REFERENCE
	
	UPROPERTY(EditAnywhere,BlueprintReadWrite, meta=(EditCondition = "TriggerAction == ETriggerAction::NewDoorLockedState",EditConditionHides), Category="Trigger System")
	bool bNewDoorLockedState;

	UPROPERTY(EditAnywhere,BlueprintReadWrite, meta=(EditCondition = "TriggerAction == ETriggerAction::MoveMonster",EditConditionHides), Category="Trigger System")
	TSoftObjectPtr<AActor> MonsterReference;

	UPROPERTY(EditAnywhere,BlueprintReadWrite, meta=(EditCondition = "TriggerAction == ETriggerAction::Fracture",EditConditionHides), Category="Trigger System")
	TSoftObjectPtr<AActor> FractureObjectReference;

	UPROPERTY(EditAnywhere,BlueprintReadWrite, meta=(EditCondition = "TriggerAction == ETriggerAction::PlaySoundLocation",EditConditionHides), Category="Trigger System")
	USoundWave* SoundReference;

	UPROPERTY(EditAnywhere,BlueprintReadWrite, meta=(EditCondition = "TriggerAction == ETriggerAction::PlaySoundLocation",EditConditionHides), Category="Trigger System")
	FVector SoundLocation;	
};


UCLASS( ClassGroup=(Custom), meta=(BlueprintSpawnableComponent) )
class GERMINATE_API UTriggerComponent : public UActorComponent
{
	GENERATED_BODY()

public:	
	// Sets default values for this component's properties
	UTriggerComponent();

protected:
	// Called when the game starts
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

	UFUNCTION(BlueprintCallable, meta=(ForceAsFunction), Category="Trigger System")
	void Trigger();

	// Loops through all the defined trigger conditions, will return true if all defined conditions are met.
	UFUNCTION(BlueprintCallable, meta=(ForceAsFunction), Category="Trigger System")
	bool CheckTriggerConditions();

	// Loops through all the defined actions and perform each defined action on referenced actors.
	UFUNCTION(BlueprintCallable, meta=(ForceAsFunction), Category="Trigger System")
	void PerformActions();

	UPROPERTY(BlueprintReadWrite, Category="Trigger system" )
	bool bWasTriggered = false;

	UPROPERTY(BlueprintReadOnly, Category="Trigger System")
	APowerGeneratorBase* PowerGenerator;
	
	UPROPERTY(EditAnywhere,BlueprintReadWrite, Category="Trigger System")
	TArray<FTriggerConditionStruct> TriggerConditions;

	UPROPERTY(EditAnywhere,BlueprintReadWrite, Category="Trigger System")
	TArray<FTriggerActionStruct> TriggerActions;
};
