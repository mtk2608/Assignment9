using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    public class GPS
    {
        public async Task<string> GetCachedLocation()
        {
            try
            {
                Location location = await Geolocation.Default.GetLastKnownLocationAsync();

                if (location != null)

                    return $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Console.WriteLine("Handle not supported on device exception");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                Console.WriteLine("Handle not enabled on device exception");
            }
            catch (PermissionException pEx)
            {
                Console.WriteLine("Handle permission exception");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to get location");
            }
            return "None";
        }


        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        public async Task<Location> GetCurrentLocation()
        {
            Location location = null;
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
            }

            catch (Exception ex)
            {

            }
            finally
            {
                _isCheckingLocation = false;
            }
            return location;

        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }
    }
}
