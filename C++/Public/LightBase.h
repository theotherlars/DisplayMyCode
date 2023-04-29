// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Components/SpotLightComponent.h"
#include "GameFramework/Actor.h"
#include "LightBase.generated.h"

UCLASS()
class GERMINATE_API ALightBase : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	ALightBase();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

	float TimeSinceLastFlicker;
	float TimeToNextFlicker;	
	
public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;
	
	// References
	UPROPERTY(VisibleAnywhere, Category="Light Base")
	USpotLightComponent* Light;

	UPROPERTY(VisibleAnywhere, Category="Light Base")
	UStaticMeshComponent* StaticMesh;
	
	// Properties
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category="Light Base")
	float OnIntensity;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category="Light Base")
	bool bIsOn;

	// Flickering Variables
	UPROPERTY(EditAnywhere, Category="Light Base")
	float MinTimeBetweenFlickers;

	UPROPERTY(EditAnywhere, Category="Light Base")
	float MaxTimeBetweenFlickers;
	
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category="Light Base")
	bool bIsFlickering;
	
	
	// Turns the light on and off
	UFUNCTION(BlueprintCallable,CallInEditor, meta=(ForceAsFunction), Category="Light Base")
	void SetNewState(bool bNewState);

	UFUNCTION(BlueprintCallable,CallInEditor, meta=(ForceAsFunction), Category="Light Base")
	void TurnOn();

	UFUNCTION(BlueprintCallable,CallInEditor, meta=(ForceAsFunction), Category="Light Base")
	void TurnOff();

	
	// Changes the flicker state of a light, start/stop
	UFUNCTION(BlueprintCallable, meta=(ForceAsFunction), Category="Light Base")
	void SetFlickerState(bool bNewFlickerState);

	// Performs the flickering
	UFUNCTION(BlueprintCallable, meta=(ForceAsFunction), Category="Light Base")
	void FlickerLight();

};
