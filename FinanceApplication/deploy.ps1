Write-Host "Building API image..."
docker build -t finance-api -f PersonalFinanceDataManager.Core/dockerfile .

Write-Host "Building Blazor image..."
docker build -t finance-blazor -f FinanceApp.Blazor/dockerfile .

Write-Host "Loading images into Minikube..."
minikube image load finance-api
minikube image load finance-blazor

Write-Host "Deploying Kubernetes resources..."

kubectl apply -f k8s/db-deployment.yaml
kubectl apply -f k8s/db-service.yaml

kubectl apply -f k8s/api-deployment.yaml
kubectl apply -f k8s/api-service.yaml

kubectl apply -f k8s/blazor-deployment.yaml
kubectl apply -f k8s/blazor-service.yaml

kubectl apply -f k8s/shared-volume-pod.yaml

kubectl apply -f k8s/hostpath-writer.yaml
kubectl apply -f k8s/hostpath-reader.yaml

Write-Host "Waiting for pods to start..."
Start-Sleep -Seconds 10

Write-Host "Starting API port-forward..."
Start-Process powershell -ArgumentList "kubectl port-forward service/api 8080:8080"

Write-Host "Opening Blazor frontend..."
Start-Process powershell -ArgumentList "minikube service blazor-service"

Write-Host "Deployment completed."