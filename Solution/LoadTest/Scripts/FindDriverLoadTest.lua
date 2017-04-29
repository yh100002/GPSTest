request = function()
  wrk.headers["Connection"] = "Keep-Alive"  
  path = "/api/drivers?Id=0&Latitude=-6.54654&Longitude=120.65454&Radius=10000&limit=10"
  return wrk.format("GET", path)
end

