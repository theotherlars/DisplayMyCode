// Fill out your copyright notice in the Description page of Project Settings.


#include "LightBase.h"
#include "Components/LightComponent.h"
#include "Kismet/KismetMathLibrary.h"

// Sets default values
ALightBase::ALightBase()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

	OnIntensity = 5000;
	bIsOn = false;
	MinTimeBetweenFlickers = 0.3f;
	MaxTimeBetweenFlickers = 2.0f;
	bIsFlickering = false;

	StaticMesh = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("LightMesh"));
	RootComponent = StaticMesh;
	
	Light = CreateDefaultSubobject<USpotLightComponent>(TEXT("SpotLight"));
	Light->SetupAttachment(RootComponent);
}

// Called when the game starts or when spawned
void ALightBase::BeginPlay()
{
	Super::BeginPlay();
	TimeToNextFlicker = UKismetMathLibrary::RandomFloatInRange(MinTimeBetweenFlickers, MaxTimeBetweenFlickers);
}

// Called every frame
void ALightBase::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);
	
	if(bIsFlickering && TimeSinceLastFlicker > TimeToNextFlicker)
	{
		FlickerLight();
		TimeToNextFlicker = UKismetMathLibrary::RandomFloatInRange(MinTimeBetweenFlickers, MaxTimeBetweenFlickers);
		TimeSinceLastFlicker = 0;
	}
	
	TimeSinceLastFlicker += DeltaTime;
}

// Performs the On/Off operations
void ALightBase::SetNewState(const bool bNewState)
{
	const float NewIntensity = bNewState ? OnIntensity : 0.0f;
	Light->SetIntensity(NewIntensity);
	bIsOn = bNewState;
}

void ALightBase::TurnOn()
{
	SetNewState(true);
}

void ALightBase::TurnOff()
{
	SetNewState(false);
}

void ALightBase::SetFlickerState(const bool bNewFlickerState)
{
	bIsFlickering = bNewFlickerState;
}

// Just a quick and dirty implementation
//TODO: Iterate on this flickering logic
void ALightBase::FlickerLight()
{
	if(UKismetMathLibrary::RandomFloat() > 0.85f) 
	{
		SetNewState(!bIsOn);
	}
}

