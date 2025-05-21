# Net Arch Tech Challenge
## 6NETT | TechChallenge 4 | Grupo 29
Link: https://youtu.be/swTEkdtt2ow

# Tech Challenge — Kubernetes Deployment Guide

## 📦 Build das Imagens Docker

Execute os comandos abaixo na raiz do repositório para realizar o build das imagens Docker de todas as aplicações:

```bash
docker build -t local_commands:latest -f dockerfiles/Dockerfile.Commands .
docker build -t local_persistence:latest -f dockerfiles/Dockerfile.Persistence .
docker build -t local_query:latest -f dockerfiles/Dockerfile.Query .
docker build -t local_dlqmonitor:latest -f dockerfiles/Dockerfile.DLQMonitor .
docker build -t local_gateway:latest -f dockerfiles/Dockerfile.Gateway .
```

---

## 🚀 Deploy no Kubernetes

### 1️⃣ Criar Namespace e Recursos Base
```bash
kubectl apply -f k8s/namespace/
kubectl apply -f k8s/base/
```

### 2️⃣ Deploy das Ferramentas (Infra)
```bash
kubectl apply -f k8s/deployments-tools/
kubectl apply -f k8s/services-tools/
kubectl apply -f k8s/ingress-tools/
```

### 3️⃣ Deploy das Aplicações
```bash
kubectl apply -f k8s/deployments-apps/
kubectl apply -f k8s/services-apps/
kubectl apply -f k8s/ingress-apps/
```

---

## 🔄 Atualizar ou Resetar Aplicações

### 🔥 Para atualizar apenas as aplicações (sem remover ferramentas/infrastrutura):
```bash
kubectl delete -f k8s/ingress-apps/
kubectl delete -f k8s/services-apps/
kubectl delete -f k8s/deployments-apps/

kubectl apply -f k8s/deployments-apps/
kubectl apply -f k8s/services-apps/
kubectl apply -f k8s/ingress-apps/
```

---

## 💣 Resetar Todo o Ambiente

### 🔥 Remover todos os recursos:
```bash
kubectl delete namespace tech-challenge
```

### 🔥 Ou deletar manualmente:
```bash
kubectl delete -f k8s/ingress-tools/
kubectl delete -f k8s/ingress-apps/
kubectl delete -f k8s/services-tools/
kubectl delete -f k8s/services-apps/
kubectl delete -f k8s/deployments-tools/
kubectl delete -f k8s/deployments-apps/
kubectl delete -f k8s/base/
kubectl delete -f k8s/namespace/
```

---

## 📜 Estrutura dos Arquivos Kubernetes (`/k8s`)

| Pasta               | Descrição                                       |
|---------------------|-------------------------------------------------|
| `namespace/`        | Namespace do projeto (`tech-challenge`)         |
| `base/`             | ConfigMaps, PVCs, secrets, e recursos base      |
| `deployments-tools/`| Deployments das ferramentas (SQL, RabbitMQ...)  |
| `deployments-apps/` | Deployments das aplicações                      |
| `services-tools/`   | Services das ferramentas (SQL, RabbitMQ...)     |
| `services-apps/`    | Services das aplicações                         |
| `ingress-tools/`    | Ingress para ferramentas (RabbitMQ, Grafana...) |
| `ingress-apps/`     | Ingress para Gateway e APIs                     |

---

## 🚀 Observações
- As imagens Docker são criadas localmente e utilizadas diretamente pelo Kubernetes (via Docker Desktop com Kubernetes habilitado).
- Se desejar rodar em clusters externos (EKS, AKS, GKE, etc.), é necessário fazer push das imagens para um registry (DockerHub, GHCR, etc.).
- O Ingress requer que você tenha um ingress controller instalado (como NGINX Ingress).

---

## 🏆 Status
✔️ Infraestrutura preparada com:
- Kubernetes
- Deploys organizados e replicáveis
- Observabilidade com Prometheus + Grafana
- Pipeline de build e deploy das aplicações
