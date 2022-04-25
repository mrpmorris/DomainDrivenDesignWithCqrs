using DomainDrivenDesignWithCqrs.Contracts.Organisations;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace DomainDrivenDesignWithCqrs.Client.Pages;

public partial class Index
{
	[Inject] private HttpClient HttpClient { get; set; } = null!;
	private readonly CreateOrganisationCommand Command = new();

	private async Task CreateAsync()
	{
		var response = await HttpClient.PostAsJsonAsync("/api/organisation/create", Command);
	}
}
