// Fill out your copyright notice in the Description page of Project Settings.


#include "TriggerComponent.h"
#include "Kismet/GameplayStatics.h"
#include "ToolBuilderUtil.h"

// Sets default values for this component's properties
UTriggerComponent::UTriggerComponent()
{
	// Set this component to be initialized when the game starts, and to be ticked every frame.  You can turn these features
	// off to improve performance if you don't need them.
	PrimaryComponentTick.bCanEverTick = true;
}


// Called when the game starts
void UTriggerComponent::BeginPlay()
{
	Super::BeginPlay();
	PowerGenerator = Cast<APowerGeneratorBase>(UGameplayStatics::GetActorOfClass(GetWorld(), APowerGeneratorBase::StaticClass()));
}


// Called every frame
void UTriggerComponent::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);
}

void UTriggerComponent::Trigger()
{
	if(bWasTriggered){ return; }

	// Checks if trigger conditions are met
	if(CheckTriggerConditions() == true)
	{
		bWasTriggered = true;
	
		// Performs all defined trigger actions 
		PerformActions();
	}
}

// Returns true if all the conditions are met else will return false
bool UTriggerComponent::CheckTriggerConditions()
{
	for (const FTriggerConditionStruct Condition : TriggerConditions)
	{
		switch (Condition.TriggerCondition)
		{
			case ETriggerCondition::GeneratorState :
				{
					if(PowerGenerator->GetActive() != Condition.bPowerGeneratorOn) {
						return false;
					}
					break;
				}
			case ETriggerCondition::OtherTriggered :
				{
					if(Condition.TriggerObjectReference->FindComponentByClass<UTriggerComponent>()->bWasTriggered != Condition.bRequiredTriggerState)
					{
						return false;
					}
				}
		// TODO: Add more conditions as they are made
			default : {
				break;
			} 
		}
	}
	
	return true;
}

// Loops through all defined actions and performs the specific action 
void UTriggerComponent::PerformActions()
{
	for (const FTriggerActionStruct Action : TriggerActions)
	{
		switch (Action.TriggerAction)
		{
			case ETriggerAction::NewLightState:
				Action.LightBaseReference->SetNewState(Action.bNewLightState);
				break;
			case ETriggerAction::LightFlicker:
				Action.LightBaseReference->SetFlickerState(Action.bNewFlickerState);
				break;
			case ETriggerAction::NewDoorLockedState:
				Action.DoorBaseReference->SetNewLockState(Action.bNewDoorLockedState);
				break;
			case ETriggerAction::MoveMonster:
				break;
			case ETriggerAction::Fracture:
				break;
			case ETriggerAction::PlaySoundLocation:
				break;
			default: ;
		}
	}
}


